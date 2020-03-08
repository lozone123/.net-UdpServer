using System;
using System.Text;
using AsyncNet.Udp.Server;
using UdpServer.CacheManage;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Udp.BLL;
using Udp.Model.Messages;
using Udp.Model.Repo;
using System.Data;
using AsyncNet.Udp.Remote.Events;
using System.Net;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace UdpServer
{
    public class Server
    {
        public static AsyncNetUdpServer mUdpServer = null;
        static readonly int PORT = 57789;

        public Server()
        {
            if (mUdpServer == null)
            {

                mUdpServer = new AsyncNetUdpServer(PORT);

                mUdpServer.ServerStarted += (s, e) => Console.WriteLine($"Server started on port: {e.ServerPort}");

                mUdpServer.ServerStopped += (s, e) =>
                {
                    Console.WriteLine($"Server stopped");
                };

                mUdpServer.ServerExceptionOccured += (s, e) =>
                {
                    Console.WriteLine($"Server Exception:{e.Exception.Message}");
                };

                mUdpServer.UdpSendErrorOccured += (s, e) =>
                {
                    Console.WriteLine($"Server Send Exception:{e.Exception.Message},{e.SendErrorType}");
                };

                mUdpServer.UdpPacketArrived += (s, e) =>
                {
                    UdpPacketArrive(e);
                };

                mUdpServer.StartAsync(CancellationToken.None);
            }
        }

        public static AsyncNetUdpServer GetUdpServerInstance()
        {
            if (mUdpServer == null)
                new Server();
            return mUdpServer;
        }

        /// <summary>
        /// 通过心跳把客户端的通信IP和端口号存入数据库，然后有新消息的时候就发送
        /// </summary>
        /// <param name="e"></param>
        private void UdpPacketArrive(UdpPacketArrivedEventArgs e)
        {
            try
            {
                string msg = Encoding.UTF8.GetString(e.PacketData);
                var m = JsonConvert.DeserializeObject<MessageModel>(msg);

                //判断用户是否已经登录
                if (!IsLogin(m.FromUid)) return;

                //获取我的消息
                GetMyMsg(m.FromUid, m.ToUid, e);

                //如果不是心跳包则新消息入库并立即发送
                if (!m.Msg.Equals("heart"))
                {
                    //入库并响应客户端
                    ResponseToClientIfMsgSendSuccess(SaveMsg(m), m.id, e);
                    //立即把消息发给朋友
                    SendMsg(m);
                }
                else
                {
                    //更新通信IP、端口
                    UserInfoBLL.UpdateUdpEndPoint(e.RemoteEndPoint.Address.ToString(), e.RemoteEndPoint.Port, m.FromUid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace + ex.Message);
            }
        }

        private void ResponseToClientIfMsgSendSuccess(bool v, string id, UdpPacketArrivedEventArgs e)
        {
            string flat = "fail";
            if (v)
            {
                flat = "ok";
            }
            var msgModel = new MessagesModel
            {
                id = id,
                msg = flat,
                read_state=-99//标志这条消息是系统自动发送,是判断客户端的消息是否成功发送的标志
            };
            var list = new List<MessagesModel>{ msgModel };
            var jsonMsg = JsonConvert.SerializeObject(list);
            mUdpServer.Post(Encoding.UTF8.GetBytes(jsonMsg), e.RemoteEndPoint);
        }

        private void SendMsg(MessageModel m)
        {
            if (m.ToUid != null && m.ToUid != "")
            {
                UserInfoModel userInfoModel = UserInfoBLL.GetModel(int.Parse(m.ToUid));
                if (userInfoModel != null
                    && !string.IsNullOrEmpty(userInfoModel.udpip)
                    && !string.IsNullOrEmpty(userInfoModel.udp_port))
                {
                    //如果客户端的通信端口信息已经超过30分钟不更新了则不再发送消息，因为超过30分钟理论上已经关闭了
                    var timeDiff = (DateTime.Now - userInfoModel.create_date.Value).TotalMinutes;
                    if (timeDiff <= 30)
                    {
                        var dtMsg = MessagesBLL.GetMessages(m.ToUid, m.FromUid);
                        if (dtMsg != null && dtMsg.Rows.Count > 0)
                        {
                            var jsonMsg = JsonConvert.SerializeObject(dtMsg);
                            var ipEndPoint = new IPEndPoint(IPAddress.Parse(userInfoModel.udpip), int.Parse(userInfoModel.udp_port));
                            mUdpServer.Post(Encoding.UTF8.GetBytes(jsonMsg), ipEndPoint);
                        }
                    }
                }
            }
        }

        private bool SaveMsg(MessageModel m)
        {
            //这是表的Model，跟MessageModel不一样，MessageModel是自定义的消息结构
            //if (MessagesBLL.Exist(m.id)) return true;
            MessagesModel tableMessages = new MessagesModel
            {
                id = m.id,
                uid = m.FromUid,
                to_uid = m.ToUid ?? "0",
                msg = m.Msg
            };
            try
            {
                return MessagesBLL.Add(tableMessages) > 0;
            }
            catch (MySqlException e)
            {
                if(e.Message.Contains("Duplicate entry"))
                {
                    return true;
                }
            }
            return false;
          
        }

        private void GetMyMsg(string fromuid, string touid, UdpPacketArrivedEventArgs e)
        {
            var dtMsg = MessagesBLL.GetMessages(fromuid, touid);
            if (dtMsg != null && dtMsg.Rows.Count > 0)
            {
                var jsonMsg = JsonConvert.SerializeObject(dtMsg);
                mUdpServer.Post(Encoding.UTF8.GetBytes(jsonMsg), e.RemoteEndPoint);
            }
        }

        private bool IsLogin(string fromUid)
        {
            var user = UserInfoBLL.GetModel(int.Parse(fromUid));

            if (user == null || user.login_state == 0)
            {
                //未登录
                return false;
            }

            return true;
        }

    }
}

