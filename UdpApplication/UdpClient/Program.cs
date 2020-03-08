using AsyncNet.Udp.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Udp.Model.Messages;
using Udp.Model.Repo;
using UdpClient.Commands;

namespace UdpClient
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var user = new UserInfoModel
            {
                id = "1",
                username = "lcz",
                nickname = "lcz",
            };
            Client.SetLoginUser(user);
            Client.Init();
            var i = 0;
            var listMsg = new List<MessageModel>();
            while (i < 10000)
            {
                i++;
                string msg = i + "互联网巨头和SaaS玩家正马不停蹄地圈地。2月19日，微盟发布公告称，" +
                    "旗下公司“微盟餐林”计划以1.14亿元收购餐饮SaaS服务商雅座，" +
                    "交易完成后合适时间将对雅座并表。仅相隔几天，2月24日，阿里本地生活服务" +
                    "公司宣布全资收购餐饮SaaS公司客如云。餐饮SaaS成香饽饽，智慧餐饮风已起微盟与阿里不约" +
                    "而同布局餐饮SaaS，或许意味着智慧餐饮市场真的起风了。客如云成立于2012年，" +
                    "专注于向本地生活服务业商家提供软硬件一体、云端结合的POS + SaaS整体解决方案。" +
                    "而阿里自身定位是各行各业数字化操作系统，阿里本地生活服务公司早已提出从传统" +
                    "“流量红利”向“数字化红利”升级的目标，收购客如云，将后者客户资源、" +
                    "产品矩阵和服务能力与饿了么到家业务、口碑到店业务、蜂鸟本地即时配送结合，" +
                    "可以更快落地数字餐饮";

                var m = new MessageModel
                {
                    id = "9" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Msg = msg,
                    ToType = 0,
                    MsgType = 0,
                    ToUid = "10",
                    FromUid = "9",
                    FromUname="lcz",
                };
                //var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(m);
                Thread.Sleep(3);
                listMsg.Add(m);
            }

           var result= Parallel.ForEach(listMsg, (s,k,index) => {
               Client.PostModel(s);
               });

            if (result.IsCompleted)
            {
                Thread.Sleep(10000);
                Client.TryPostModelAgain();
            }

            Console.WriteLine(result.IsCompleted);
            //Console.WriteLine(result.LowestBreakIteration.Value);

            Console.ReadKey();

            //System.Timers.Timer t = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为10000毫秒；

            //t.Elapsed += new System.Timers.ElapsedEventHandler((source,e)=>
            //{
            //    Client.Send("heart");
            //});//到达时间的时候执行事件；

            //t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            //t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            //CommandExecutor cmdExecutor = new CommandExecutor(new CommandMatcher());
            //Console.WriteLine("等待输入：");
            //while (true)
            //{
            //    var strInput = Console.ReadLine();
            //    if (!string.IsNullOrEmpty(strInput))
            //    {
            //        cmdExecutor.Exec(strInput);
            //    }
            //    Console.WriteLine("等待输入：");
            //}
        }
    }
}
