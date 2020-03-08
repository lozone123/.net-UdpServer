using System;
using System.Collections.Generic;
using System.Text;

namespace Udp.Model.Repo
{
    public class UserInfoModel
    {
        public string id;
        public string username { get; set; }
        public string userpass { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        /// <summary>
        /// 登录状态，0未登录，1：已经登录
        /// </summary>
        public int login_state { get; set; }
        public string udpip { get; set; }
        public string udp_port { get; set; }
        public DateTime? create_date { get; set; }
    }
}
