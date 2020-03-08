using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
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

        public string Reg(UserInfoModel info, string code)
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

        public string QuickReg(UserInfoModel info)
        {
            var error = new ErrorMessage();
            if (string.IsNullOrEmpty(info.username) || string.IsNullOrEmpty(info.userpass))
            {
                error.State = 1;
                error.Msg = "name can't be empty";
            }
            else if (UserInfoBLL.IsExistUser<UserInfoModel>(info.username))
            {
                error.State = 1;
                error.Msg = "name already exist";
            }
            else
            {
                info.userpass = Udp.Security.DESEncrypt.DesEncrypt(info.userpass);
                var succ = UserInfoBLL.Add(info,1);
                if (!succ)
                {
                    error.State = 3;
                    error.Msg = "Enter name failed,please try again.";
                }
                else
                {
                    error.State = 0;
                    DataTable dt=UserInfoBLL.GetUser(info.username);
                    error.Msg = dt.Rows[0]["id"].ToString();
                    error.Msg2 = dt.Rows[0]["nickname"].ToString();
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(error);
        }

        public string GetAdmin()
        {
            string username = "admin";
            string nickname = "China Train Assistant";

            var dt = UserInfoBLL.GetUser(username);
            if(dt==null || dt.Rows.Count == 0)
            {
                var userModel = new UserInfoModel
                {
                    username=username,
                    nickname=nickname,
                    userpass="aaabbb123",
                };
                return QuickReg(userModel);
            }
            else
            {
                var error = new ErrorMessage();
                error.State = 0;
                error.Msg = dt.Rows[0]["id"].ToString();
                error.Msg2 = dt.Rows[0]["nickname"].ToString();
                return Newtonsoft.Json.JsonConvert.SerializeObject(error);
            }

        }

        public string ConfirmMsg(string msgids)
        {
            if (!string.IsNullOrEmpty(msgids))
            {
               return MessagesBLL.UpdateReadStateBatchByMsgIds(1, msgids).ToString();
            }
            return "false";
        }

        public string GetNewMsg(string toUid)
        {
            if (!string.IsNullOrEmpty(toUid))
            {
                var dt = MessagesBLL.GetNewMsg(toUid);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
            }
            return "";
        }

        public string CheckNewMsg(string toUid)
        {
            if (!string.IsNullOrEmpty(toUid))
            {
               return MessagesBLL.CheckNewMsg(toUid);
            }
            return "0";
        }
    }
}