using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class AddFriendCommand : ICommand
    {
        public void Exec(string cmd)
        {
        }

        public string GetComand()
        {
            return "addfriend";
        }
    }
}
