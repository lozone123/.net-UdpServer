using System;
using System.Collections.Generic;
using System.Text;
using Udp.Model;
using UdpClient.Utils;

namespace UdpClient.Commands
{
    public class RegCommand : ICommand
    {
        const string CommandReg = @"reg\s+(.+?)\s+(.+)?";
        public void Exec(string cmd)
        {
            string[] data = cmd.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 4)
            {
                //获取验证码
                string code = HttpUtil.Get(ApiConfig.host + "/auth/GetRandCode/");
                if (!string.IsNullOrEmpty(code))
                {
                    code = Udp.Security.DESEncrypt.DesDecrypt(code);
                    Console.WriteLine($"请输入验证码（{code}）");
                }
            }
            else if (data.Length ==5)
            {
                string param = $"username={data[1]}&userpass={data[2]}&nickname={data[3]}&code={data[4]}";
                string str = HttpUtil.Get(ApiConfig.host + "/auth/reg?" + param);
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorMessage>(str);
                if (error.State > 0)
                {
                    Console.WriteLine(error.Msg);
                }
                else
                {
                    Console.WriteLine("注册成功，请登录，登录命令格式：login <username> <password>");
                }
            }
            else
            {
                Console.WriteLine("注册命令格式不正确，应该是(reg 用户名 密码 昵称)");
            }
        }

        public string GetComand()
        {
            return CommandReg;
        }
    }
}
