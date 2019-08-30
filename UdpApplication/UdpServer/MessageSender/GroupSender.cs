using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Udp.Model.Messages;
using UdpServer.CacheManage;


namespace UdpServer.MessageSender
{
    public class GroupSender:Sender
    {
        public GroupSender()
        {
            var sendThread = new Thread(() =>
            {
                var i = 0;
                Dictionary<string, string> groupMsg = new Dictionary<string, string>();
                while (true)
                {
                    i++;
                    if (MessageQueue.msgQueueGroup.Count <= 0 || i % 1000 == 0)
                    {
                        Thread.Sleep(2000);
                        i = 0;
                    }
                    if (MessageQueue.msgQueueGroup.Count <= 0)
                        continue;
                    var msg = MessageQueue.msgQueueGroup.Dequeue();
                    try
                    {
                        var model = JsonConvert.DeserializeObject<MessageModel>(msg);
                        //群消息进行分类，同一个群的消息串在一起然后一起发送
                        if (groupMsg.ContainsKey(model.ToUid))
                        {
                            var msg2 = groupMsg[model.ToUid];
                            groupMsg[model.ToUid] = msg2 + "," + msg;
                        }
                        else
                        {
                            groupMsg.Add(model.ToUid, msg);
                        }
                        //群消息每两百条发送一次
                        if (MessageQueue.msgQueueGroup.Count <= 0 || i%200==0)
                        {
                            foreach (var key in groupMsg.Keys)
                            {
                                //这里需要获取所有群成员的ipEndPoint，可以通过数据库获取
                                var toUser = UserCache.GetUser(key);
                                if (toUser != null)
                                {
                                    SendMessage(groupMsg[model.ToUid], toUser);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            sendThread.Start();
        }

        public override bool SendMessage(string msg, IPEndPoint toUser)
        {
            return base.SendMessage(msg, toUser);
        }

    }
}
