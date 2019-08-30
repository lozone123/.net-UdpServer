using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net;

namespace UdpClient.Utils
{
    public class HttpUtil
    {
        public static string Get(string url)
        {
            var webClient = new WebClient();
            string str = webClient.DownloadString(url);
            return str;
        }
    }
}
