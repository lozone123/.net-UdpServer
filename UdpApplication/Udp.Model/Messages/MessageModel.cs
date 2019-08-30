using System;
using System.Collections.Generic;
using System.Text;

namespace Udp.Model.Messages
{
    public class MessageModel
    {
        public string Msg { get; set; }
        /// <summary>
        /// 文本或者视频、语音等
        /// </summary>
        public int MsgType { get; set; }
        /// <summary>
        /// 0发送给个人，1群消息
        /// </summary>
        public int ToType { get; set; }
        public string FromUid { get; set; }
        public string FromUname { get; set; }
        public string ToUid { get; set; }
    }
}
