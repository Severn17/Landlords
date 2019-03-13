using System;
using AhpilyServer;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            server.Start(6666, 10);
            Console.ReadKey();
        }
    }
}
