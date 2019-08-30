using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Udp.Model.EnumPakcet;

namespace Udp.Model.Messages
{
    public class MessageQueue
    {
        public static Queue<string> msgQueuePersonal = new Queue<string>();
        public static Queue<string> msgQueueGroup = new Queue<string>();

        public static void AddMsg(string msg,MessageModel m)
        {
            try
            {
                if (m != null)
                {
                    if (m.ToType == (int)ToTypeEnum.personal)
                    {
                        msgQueuePersonal.Enqueue(msg);
                    }
                    else
                    {
                        msgQueueGroup.Enqueue(msg);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
      
    }
}
