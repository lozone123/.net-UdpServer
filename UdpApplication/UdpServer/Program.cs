using AsyncNet.Udp.Server;
using System;
using System.Threading;
using UdpServer.MessageSender;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server();
            new PersonalSender();
            new GroupSender();
        }
    }
}
