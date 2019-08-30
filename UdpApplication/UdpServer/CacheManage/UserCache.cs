using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UdpServer.CacheManage
{
    public class UserCache
    {
        static Dictionary<string,IPEndPoint> usersDic =new Dictionary<string, IPEndPoint>();

        public static IPEndPoint GetUser(string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return null;
            if (usersDic.ContainsKey(uid))
                return usersDic[uid];
            return null;
        }
        public static void AddUser(string uid, IPEndPoint endPoint)
        {
            if (string.IsNullOrEmpty(uid) || endPoint==null)
                return;
            if (usersDic.ContainsKey(uid))
                usersDic.Remove(uid);

            usersDic.Add(uid, endPoint);
        }

        public static void RemoveUser(string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return;
            if (usersDic.ContainsKey(uid))
                usersDic.Remove(uid);
        }
    }
}
