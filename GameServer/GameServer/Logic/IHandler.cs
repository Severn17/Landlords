using AhpilyServer;
using System;
using System.Collections.Generic;


namespace GameServer.Logic
{
    public interface IHandler
    {
        void OnReceive(ClientPeer client, int subCode,object value);
        void OnDisconnect(ClientPeer client);
    }
}
