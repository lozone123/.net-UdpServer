using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class LogOutCommand : ICommand
    {
        public void Exec(string cmd)
        {
            
        }

        public string GetComand()
        {
            return "logout";
        }
    }
}
