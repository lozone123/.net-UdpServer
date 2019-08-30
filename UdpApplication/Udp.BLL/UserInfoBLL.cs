using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Udp.Model.Repo;
using UdpServer.MySqlManage;

namespace Udp.BLL
{
    public class UserInfoBLL
    {
        public static UserInfoModel GetModel(int id)
        {
            string sql = "select * from userinfo where id=@id";
            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("id", id);

            DataTable dt = MysqlTool.ExecuteDataTable(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                var dataJson = JsonConvert.SerializeObject(dt);
                var model = JsonConvert.DeserializeObject<List<UserInfoModel>>(dataJson);
                return model[0];
            }
            return null;
        }

        public static DataTable GetUser(string username)
        {
            string sql = "select * from userinfo where username=@username";
            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("username", username);

            DataTable dt = MysqlTool.ExecuteDataTable(sql, param);
            return dt;
        }

        public static UserInfoModel GetModel(string username,string pass)
        {
            string sql = "select * from userinfo where username=@username and userpass=@pass";
            MySqlParameter[] param = new MySqlParameter[2];
            param[0] = new MySqlParameter("username", username);
            param[1] = new MySqlParameter("pass", pass.Replace(" ","+"));

            DataTable dt = MysqlTool.ExecuteDataTable(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                var dataJson = JsonConvert.SerializeObject(dt);
                var model = JsonConvert.DeserializeObject<List<UserInfoModel>>(dataJson);
                return model[0];
            }
            return null;
        }

        public static DataTable GetUserOnline()
        {
            string sql = "select * from userinfo where login_state=1";
     
            DataTable dt = MysqlTool.ExecuteDataTable(sql);

            return dt;
        }

        public static bool UpdateLoginState(int loginState)
        {
            var sql = "update userinfo set login_state=@login_state";
            var param = new MySqlParameter[1]
            {
                new MySqlParameter("login_state",loginState),
            };
            var ret = MysqlTool.ExecuteNonQuery(sql, param);
            return ret > 0;
        }

        public static bool Add(UserInfoModel model)
        {
            var sql = "insert into userinfo(username,userpass,nickname,login_state) values(@username,@userpass,@nickname,0)";
            var param = new MySqlParameter[3];
            param[0] = new MySqlParameter("username", model.username);
            param[1] = new MySqlParameter("userpass", model.userpass);
            param[2] = new MySqlParameter("nickname", model.nickname);
            var ret = MysqlTool.ExecuteNonQuery(sql, param);
            return ret > 0;
        }

        public static bool IsExistUser<T>(string username) where T:UserInfoModel
        {
            var param = new MySqlParameter[1];
            param[0] = new MySqlParameter("username", username);
            return IsExist<T>(x => x.username == username,param);
        }
        private static bool IsExist<T>(Expression<Func<T,bool>> expression,MySqlParameter[] param) where T:UserInfoModel
        {
            string where = ExpressionTransform.DealExpress(expression);
            string sql = $"select count(1) from userinfo where {where}";
            var dt = MysqlTool.ExecuteDataTable(sql,param);
            if(dt!=null && dt.Rows.Count > 0)
            {
                var total = int.Parse(dt.Rows[0][0].ToString());
                if (total > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsExistNickName<T>(string nickname) where T:UserInfoModel
        {
            var param = new MySqlParameter[1];
            param[0] = new MySqlParameter("nickname", nickname);
            return IsExist<T>(x => x.nickname == nickname,param);
        }
    }
}
