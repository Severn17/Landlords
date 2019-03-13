using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using AhpilyServer;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            server.Start(6666,10);
            Console.ReadKey();
        }
    }
}
