using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Udp.BLL;
using Udp.Model;
using Udp.Model.Repo;
using UdpWeb.Utils;

namespace UdpWeb.Controllers
{
    public class AuthController : Controller
    {
        static Dictionary<string, string> dicCode = new Dictionary<string, string>();
        // GET: Login
        public string Login(string username, string password)
        {
            var error = new ErrorMessage();
            if (string.IsNullOrEmpty(username) 
                || string.IsNullOrEmpty(password))
            {
                error.State = 1;
                error.Msg = "用户名或者密码不可以为空";
            }
            else
            {
                var userInfo = UserInfoBLL.GetModel(username, password);
                if (userInfo == null)
                {
                    error.State = 2;
                    error.Msg = "用户不存在或者密码错误";
                }
                else
                {
                    UserInfoBLL.UpdateLoginState(1);
                    userInfo.login_state = 1;
                    error.State = 0;
                    error.Msg = Newtonsoft.Json.JsonConvert.SerializeObject(userInfo);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(error);
        }

        public string Reg(UserInfoModel info,string code)
        {
            var error = new ErrorMessage();
            if (string.IsNullOrEmpty(info.username) || string.IsNullOrEmpty(info.userpass))
            {
                error.State = 1;
                error.Msg = "用户名或者昵称不可以为空";
            }
            else if (UserInfoBLL.IsExistUser<UserInfoModel>(info.username))
            {
                error.State = 1;
                error.Msg = "用户名已经存在";
            }
            else if (UserInfoBLL.IsExistNickName<UserInfoModel>(info.nickname))
            {
                error.State = 2;
                error.Msg = "昵称已经存在";
            }
            else
            {
                if (string.IsNullOrEmpty(code) || !dicCode.ContainsKey(code))
                {
                    error.State = 2;
                    error.Msg = "验证码错误";
                }
                else
                {
                    dicCode.Remove(code);
                    info.userpass = Udp.Security.DESEncrypt.DesEncrypt(info.userpass);
                    var succ = UserInfoBLL.Add(info);
                    if (!succ)
                    {
                        error.State = 3;
                        error.Msg = "注册失败,请检查填写的信息是否正确";
                    }
                    else
                    {
                        error.State = 0;
                    }
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(error);
        }

        public string GetRandCode()
        {
            var validateObj = new ValidateCode();
            var code = validateObj.CreateValidateCode(4);
            if (!dicCode.ContainsKey(code))
            {
                dicCode.Add(code, "");
            }
            code = Udp.Security.DESEncrypt.DesEncrypt(code);
            return code;
        }
    }
}