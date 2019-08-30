using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Udp.BLL;
using UdpWeb.Models;

namespace UdpWeb.Controllers
{
    public class FriendController : Controller
    {
       public string GetOnlineFriend()
        {
            var dt = UserInfoBLL.GetUserOnline();
            if(dt!=null && dt.Rows.Count > 0)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            }
            return "";
        }
        
    }
}
