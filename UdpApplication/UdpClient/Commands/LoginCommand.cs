using System;
using System.Collections.Generic;
using System.Text;
using Udp.Model;
using Udp.Model.Repo;
using UdpClient.Utils;

namespace UdpClient.Commands
{
    public class LoginCommand : ICommand
    {
        const string CommandReg = @"login\s+(.+?)\s+(.+)?";
        public void Exec(string cmd)
        {
            string[] data = cmd.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 3)
            {
                var password = Udp.Security.DESEncrypt.DesEncrypt(data[2]);
                string param = $"username={data[1]}&password={password}";
                string str = HttpUtil.Get(ApiConfig.host + "/auth/login?" + param);
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorMessage>(str);
                if (error.State > 0)
                {
                    Console.WriteLine(error.Msg);
                }
                else
                {
                    var m = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoModel>(error.Msg);
                    Client.SetLoginUser(m);
                    Client.Init();
                    Console.WriteLine("登录成功，可查看当前在线的朋友并开始聊天，命令格式：list online");
                }
            }
        }

        public string GetComand()
        {
            return CommandReg;
        }
    }
}
