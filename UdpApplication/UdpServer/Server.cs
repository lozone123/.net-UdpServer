using AsyncNet.Udp.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Udp.BLL;
using Udp.Model.Messages;
using UdpServer.CacheManage;

namespace UdpServer
{
    public class Server
    {
        public static AsyncNetUdpServer udpServer = null;
        public Server()
        {
            if (udpServer == null)
            {
                udpServer = new AsyncNetUdpServer(7788);
                udpServer.ServerStarted += (s, e) => Console.WriteLine($"Server started on port: {e.ServerPort}");
                udpServer.UdpPacketArrived += (s, e) =>
                {
                    Console.WriteLine($"Server received: " +
                        $"{System.Text.Encoding.UTF8.GetString(e.PacketData)} " +
                        "from " +
                        $"[{e.RemoteEndPoint}]");
                    try
                    {
                        string msg = Encoding.UTF8.GetString(e.PacketData);
                        var m = JsonConvert.DeserializeObject<MessageModel>(msg);
                        //判断用户是否已经登录
                        var user = UserInfoBLL.GetModel(int.Parse(m.FromUid));
                        if (user == null || user.login_state == 0)
                        {
                            return;
                        }
                        MessageQueue.AddMsg(msg,m);
                        UserCache.AddUser(m.FromUid, e.RemoteEndPoint);
                    }
                    catch (Exception)
                    {
                    }
                };
                udpServer.StartAsync(CancellationToken.None);
            }
        }

        public static AsyncNetUdpServer GetUdpServerInstance()
        {
            if (udpServer == null)
                new Server();
            return udpServer;
        }

    }
}
