using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class SendMessageCommand : ICommand
    {
        public void Exec(string cmd)
        {
            if (Client.Send(cmd))
            {
                Console.WriteLine($"{Client.GetLoginUser().nickname}：{cmd}");
            }
            else
            {
                Console.WriteLine("发送失败，请稍后重试");
            }
        }

        public string GetComand()
        {
            return null;
        }
    }
}
