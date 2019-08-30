using AsyncNet.Udp.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Udp.Model.Messages;
using Udp.Model.Repo;

namespace UdpClient
{
    public class Client
    {
        const string HOST = "127.0.0.1";
        const int PORT = 7788;

        static UserInfoModel userInfo;
        static UserInfoModel toUser;
        static AsyncNetUdpClient udpClient;

        public static void Init()
        {
            if (udpClient == null)
            {
                udpClient = new AsyncNetUdpClient(HOST, PORT);

                udpClient.ClientReady += (s, e) =>
                {
                    var msgModel = new MessageModel
                    {
                        Msg = "登录成功",
                        FromUid = userInfo.id.ToString(),
                        FromUname = userInfo.nickname,
                    };
                    var strMsg = Newtonsoft.Json.JsonConvert.SerializeObject(msgModel);
                    udpClient.Post(Encoding.UTF8.GetBytes(strMsg));
                };

                udpClient.UdpPacketArrived += (s, e) =>
                {
                    var recMsg = Encoding.UTF8.GetString(e.PacketData);
                    try
                    {
                        var model = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(recMsg);
                        if (model != null)
                        {
                            AddChattingFriend(model);
                            string showMsg = $"{model.FromUname}：{model.Msg}";
                            Console.WriteLine(showMsg);
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
            var m = new MessageModel
            {
                FromUid = userInfo.id.ToString(),
                FromUname = userInfo.nickname,
                ToType = 0,
                ToUid = toUser.id.ToString(),
                Msg=msg,
            };
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            return udpClient.Post(Encoding.UTF8.GetBytes(jsonStr));
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
