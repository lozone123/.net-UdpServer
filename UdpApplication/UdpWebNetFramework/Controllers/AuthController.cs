using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udp.BLL;
using Udp.Model;
using Udp.Model.Repo;

namespace UdpWeb.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public string Login(string username, string password, string nickname)
        {
            var error = new ErrorMessage();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                error.State = 1;
                error.Msg = "用户名或者昵称不可以为空";
            }
            else if (UserInfoBLL.IsExistUser<UserInfoModel>(username))
            {
                error.State = 1;
                error.Msg = "用户名已经存在";
            }
            else if (UserInfoBLL.IsExistNickName<UserInfoModel>(nickname))
            {
                error.State = 2;
                error.Msg = "昵称已经存在";
            }
            else
            {
                error.State = 0;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(error);
        }

        public string Reg(UserInfoModel info)
        {
            if (string.IsNullOrEmpty(info.username)
                 || string.IsNullOrEmpty(info.userpass)
                 || string.IsNullOrEmpty(info.nickname))
            {
                return "";
            }
            return "";
        }
    }
}