using System;
using AhpilyServer;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            // 指定所关联的应用层
            server.SetApplication(new NetMsgCenter());
            server.Start(6666, 10);
            Console.ReadKey();
        }
    }
}
