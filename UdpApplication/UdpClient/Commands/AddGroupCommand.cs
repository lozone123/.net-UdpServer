using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class AddGroupCommand : ICommand
    {
        public void Exec(string cmd)
        {
           
        }

        public string GetComand()
        {
            return "addgroup";
        }
    }
}
