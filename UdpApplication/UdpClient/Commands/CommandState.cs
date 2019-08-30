using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class CommandState
    {
        //waitting:等待输入，chatting:正在聊天
        public static string Watting= "waitting";
        public static string Chatting = "chatting";

        public static string InputCode = "inputing code";
    }
}
