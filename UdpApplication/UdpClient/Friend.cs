using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Udp.Model.Repo;

namespace UdpClient
{
    public class Friend
    {
        static List<UserInfoModel> friendList = new List<UserInfoModel>();
        static List<UserInfoModel> chatFriendList = new List<UserInfoModel>();
        public static void SetOnlineFriend(List<UserInfoModel> list)
        {
            friendList = list;
        }
        public static UserInfoModel GetFriend(string nickname)
        {
            var user = friendList.Find(x => x.nickname == nickname);
            if (user == null)
            {
                user = chatFriendList.Find(x => x.nickname == nickname);
            }
            return user;
        }
        public static void AddChatFriend(UserInfoModel user)
        {
            if (!chatFriendList.Contains(user))
            {
                chatFriendList.Add(user);
            }
        }
    }
}
