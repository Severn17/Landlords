using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logic
{
    public interface IHandler
    {
        void OnReceive(ClientPeer client, int subCode,object value);
        void OnDisconnect(ClientPeer client);
    }
}
