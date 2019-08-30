using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class SelectFriendCommand : ICommand
    {
        const string CommandReg = @"select\s+.+";
        public void Exec(string cmd)
        {
            string[] data = cmd.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 2)
            {
                var friend = Friend.GetFriend(data[1]);
                if (friend == null)
                {
                    Console.WriteLine("找不到该朋友，是否名称写错了");
                }
                else
                {
                    Client.SetToUser(friend);
                    Console.WriteLine($"正在与{data[1]}聊天...");
                }
            }
        }

        public string GetComand()
        {
            return CommandReg;
        }
    }
}
