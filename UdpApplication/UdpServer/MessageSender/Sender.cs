using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UdpServer.MessageSender
{
    public class Sender
    {
        public virtual bool SendMessage(string msg,IPEndPoint toUser)
        {
            if (string.IsNullOrEmpty(msg))
                return false;
            var udpServer = Server.GetUdpServerInstance();
            return udpServer.Post(Encoding.UTF8.GetBytes(msg), toUser);
        }
    }
}
