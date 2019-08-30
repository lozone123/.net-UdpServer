using AsyncNet.Udp.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdpClient.Commands;

namespace UdpClient
{
    class Program
    {
        static void Main(string[] args)
        {
           
            //压力测试
            //Task.Run(() =>
            //{
            //    var i = 0;
            //    while (i < 10000)
            //    {
            //        i++;
            //        string msg = "this is my first time go to here." + i;
            //        var m = new Model.MessageModel
            //        {
            //            Msg = msg,
            //            ToType = 0,
            //            MsgType = 0,
            //            ToUid = "jack",
            //            FromUid = "mile"
            //        };
            //        var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            //        client.Post(Encoding.UTF8.GetBytes(jsonStr));
            //    }
            //});
            //Client.Init();
            CommandExecutor cmdExecutor = new CommandExecutor(new CommandMatcher());
            Console.WriteLine("等待输入：");
            while (true)
            {
                var strInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(strInput))
                {
                    cmdExecutor.Exec(strInput);
                }
                Console.WriteLine("等待输入：");
            }
        }
    }
}
