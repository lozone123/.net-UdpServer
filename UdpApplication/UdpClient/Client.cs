using AsyncNet.Udp.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Udp.Model.Messages;
using Udp.Model.Repo;

namespace UdpClient
{
    public class Client
    {
        const string HOST = "49.235.164.72";
        //const string HOST = "127.0.0.1";
        //const int PORT = 57788;
        const int PORT = 57789;

        static UserInfoModel userInfo;
        static UserInfoModel toUser;
        static AsyncNetUdpClient udpClient;

        static ConcurrentDictionary<string, MessageModel> dicMsgModels = new ConcurrentDictionary<string, MessageModel>();

        public static void Init()
        {
            if (udpClient == null)
            {
                udpClient = new AsyncNetUdpClient(HOST, PORT);

                udpClient.ClientReady += (s, e) =>
                {
                    //var msgModel = new MessageModel
                    //{
                    //    Msg = "登录成功",
                    //    FromUid = userInfo.id.ToString(),
                    //    FromUname = userInfo.nickname,
                    //};
                    //var strMsg = Newtonsoft.Json.JsonConvert.SerializeObject(msgModel);
                    //udpClient.Post(Encoding.UTF8.GetBytes(strMsg));
                };

                udpClient.UdpPacketArrived += (s, e) =>
                {
                    var recMsg = Encoding.UTF8.GetString(e.PacketData);
                    try
                    {
                        var modelList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessagesModel>>(recMsg);
                        if (modelList != null && modelList.Count > 0)
                        {
                            //AddChattingFriend(modelList[0]);
                            foreach (var model in modelList)
                            {
                                string showMsg = $"{model.id}：{model.msg}";
                                if (model.read_state == -99)
                                {
                                    if (model.msg.Equals("ok"))
                                    {
                                        dicMsgModels.TryRemove(model.id, out MessageModel m);
                                    }
                                }
                                Console.WriteLine(showMsg);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                };

                udpClient.StartAsync(CancellationToken.None);
            }
        }

        private static void AddChattingFriend(MessageModel model)
        {
            var chattingFriend = new UserInfoModel
            {
                id = model.FromUid,
                nickname = model.FromUname,
            };
            Friend.AddChatFriend(chattingFriend);
        }

        public static bool Send(string msg)
        {
            if (userInfo == null || toUser == null)
            {
                return false;
            }
            var m = new MessageModel
            {
                FromUid = userInfo.id.ToString(),
                FromUname = userInfo.nickname,
                ToType = 0,
                ToUid = toUser.id.ToString(),
                Msg = msg,
            };
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            return udpClient.Post(Encoding.UTF8.GetBytes(jsonStr));
        }

        public static bool SendAsync(string msg)
        {
            udpClient.SendAsync(Encoding.UTF8.GetBytes(msg));
            return false;
        }

        public static bool Post(string msg)
        {
            if (!udpClient.Post(Encoding.UTF8.GetBytes(msg)))
            {
                throw new Exception("full..................");
            }
            return false;
        }
        public static bool PostModel(MessageModel model)
        {
            dicMsgModels.TryAdd(model.id, model);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            if (!udpClient.Post(Encoding.UTF8.GetBytes(msg)))
            {
                throw new Exception("full..................");
            }
            return false;
        }

        public static bool PostModelT(MessageModel model)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            if (!udpClient.Post(Encoding.UTF8.GetBytes(msg)))
            {
                throw new Exception("full..................");
            }
            return false;
        }

        public static void TryPostModelAgain()
        {
            System.Timers.Timer t = new System.Timers.Timer(15000);//实例化Timer类，设置间隔时间为10000毫秒；

            t.Elapsed += new System.Timers.ElapsedEventHandler((source, e) =>
            {
                Task.Run(() =>
                {
                    foreach(KeyValuePair<string,MessageModel> kv in dicMsgModels)
                    {
                        PostModelT(kv.Value);
                    }
                });
            });//到达时间的时候执行事件；

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }
        public static AsyncNetUdpClient GetUdpClientInstance()
        {
            if (udpClient == null)
                Init();
            return udpClient;
        }

        public static void SetLoginUser(UserInfoModel user)
        {
            userInfo = user;
        }
        public static void SetToUser(UserInfoModel user)
        {
            toUser = user;
        }

        public static UserInfoModel GetLoginUser()
        {
            return userInfo;
        }

        public static UserInfoModel GetToUser()
        {
            return toUser;
        }
    }
}
