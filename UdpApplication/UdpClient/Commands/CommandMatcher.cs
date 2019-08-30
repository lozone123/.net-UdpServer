using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UdpClient.Commands
{
    public class CommandMatcher : ICommandMatcher
    {
        static List<ICommand> commands = new List<ICommand>()
        {
            new LoginCommand(),
            new LogOutCommand(),
            new AddFriendCommand(),
            new AddGroupCommand(),
            new SelectFriendCommand(),
            new RegCommand(),
            new ListOnlineCommand(),
        };
        public ICommand Match(string cmd)
        {
            cmd = cmd.Trim().ToLower();
            Regex reg;
            foreach (var command in commands)
            {
                var regCmd = command.GetComand();
                reg = new Regex(regCmd);
                var match = reg.Match(cmd);
                if (match.Success)
                {
                    return command;
                }
            }
            //默认是在聊天
            if (Client.GetLoginUser() != null && Client.GetToUser() != null)
            {
                return new SendMessageCommand();
            }
            return null;
        }
    }
}
