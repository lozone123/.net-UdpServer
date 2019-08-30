# .net-UdpServer

包含客户端、服务端，大家可以下载下来进行测试，目前服务端已经实现个人对个人信息的发送，群发送也实现大部分，但还不完善。其中使用到数据库Mysql,已经把数据库备份放到项目UdpWeb文件夹mysqldb里，大家需要先安装mysql，然后把数据库导进去。

其中一个项目UdpWeb主要提供注册、登录等功能

使用步骤：
先启动UdpWeb项目，UdpServer项目，然后启动UdpClient项目

在udpclient实现的方式是命令行窗口的方式，所以操作都是基于命令行的。


1、注册
reg <username> <password> <nickname>
  提交注册之后，还需要填写验证码，直接输入验证码后即可

2、登录
login <username> <password>
  
3、显示在线的朋友
list online

4、选择要聊天的朋友
select <friend's name>

5、开始聊天
直接输入文字即可

![Image text](https://github.com/lozone123/.net-UdpServer/Raw/master/readimg/20190830130801.png)
![Image text](https://github.com/lozone123/.net-UdpServer/Raw/master/readimg/20190830132052.png)
