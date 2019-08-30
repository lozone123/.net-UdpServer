using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClient.Commands
{
    public class CommandExecutor
    {
        ICommandMatcher _commandMatcher;
        ICommand command;
        string CurrentState = string.Empty;
        string prevCommand = string.Empty;
        public CommandExecutor(ICommandMatcher commandMatcher)
        {
            _commandMatcher = commandMatcher;
        }
        public void Exec(string cmd)
        {
            //等待输入验证码，注册时需要把前一条命令的信息一起串联起来处理
            if (CurrentState.Equals(CommandState.InputCode))
            {
                if (!cmd.StartsWith("reg"))
                {
                    cmd = prevCommand + " " + cmd;
                    prevCommand = "";
                }
                else
                {
                    prevCommand = cmd;
                }
            }
            else
            {
                prevCommand = cmd;
            }
            command = _commandMatcher.Match(cmd);
            if (command != null)
            {
                if (command is RegCommand)
                {
                    CurrentState = CommandState.InputCode;
                }
                else
                {
                    CurrentState = CommandState.Watting;
                }
                command.Exec(cmd);
            }
            else
            {
                Console.WriteLine("输入的命令无效，请检查输入的命令及参数是否正确");
            }
        }
    }
}
