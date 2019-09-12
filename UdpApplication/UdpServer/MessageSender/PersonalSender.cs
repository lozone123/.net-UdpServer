using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Udp.Model.Messages;

namespace UdpServer.MessageSender
{
    public class PersonalSender:Sender
    {
        public PersonalSender()
        {
            var sendThread = new Thread(() =>
            {
                var i = 0;

                while (true)
                {
                    i++;
                    //每循环一千次暂停2秒，避免CPU使用过高
                    if (GetMessageCount() <= 0 || i % 1000 == 0)
                    {
                        Thread.Sleep(2000);
                        i = 1;
                    }

                    if ( GetMessageCount()<= 0)
                        continue;

                    var msg = MessageQueue.msgQueuePersonal.Dequeue();

                    try
                    {
                        var model = JsonConvert.DeserializeObject<MessageModel>(msg);

                        var toUser = CacheManage.UserCache.GetUser(model.ToUid);

                        if (toUser != null)
                        {
                            SendMessage(msg, toUser);
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

        private int GetMessageCount()
        {
            return MessageQueue.msgQueuePersonal.Count;
        }
    }
}
