using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Udp.Model.Repo;
using UdpClient.Utils;

namespace UdpClient.Commands
{
    public class ListOnlineCommand : ICommand
    {
        const string CommandReg = @"list\s+online";
        public void Exec(string cmd)
        {
            string onlineUsers = HttpUtil.Get(ApiConfig.host+ "/Friend/GetOnlineFriend");
            if (!string.IsNullOrEmpty(onlineUsers))
            {
                var dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(onlineUsers);
                var jsonUsers = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var listUser = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserInfoModel>>(jsonUsers);
                Friend.SetOnlineFriend(listUser);
                StringBuilder builder = new StringBuilder();
                foreach(DataRow dr in dt.Rows)
                {
                    if (dr["nickname"].ToString().Trim().Equals(Client.GetLoginUser().nickname))
                    {
                        continue;
                    }
                    builder.Append(dr["nickname"]);
                    builder.Append(" ");
                }
                Console.WriteLine(builder.ToString().Trim());
            }
        }

        public string GetComand()
        {
            return CommandReg;
        }
    }
}
