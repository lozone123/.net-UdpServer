using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public interface ICommand
    {
        string GetComand();
        void Exec(string cmd);
    }
}
