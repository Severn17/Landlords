using AhpilyServer;
using GameServer.Logic;
using Protocol.Code;
using System;
using System.Collections.Generic;


namespace GameServer
{
    /// <summary>
    /// 网路消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        IHandler account = new AccountHandler();

        public void OnDisconnect(ClientPeer client)
        {
            account.OnDisconnect(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    account.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                default:
                    break;
            }

        }
    }
}
