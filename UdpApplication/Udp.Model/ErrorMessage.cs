using System;
using System.Collections.Generic;
using System.Text;

namespace Udp.Model
{
    public class ErrorMessage
    {
        /// <summary>
        /// 0:成功，其它错误
        /// </summary>
       public int State { get; set; }
       public string Msg { get; set; }
       public string Msg2 { get; set; }
    }
}
