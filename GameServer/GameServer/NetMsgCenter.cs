using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 网路消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        public void OnDisconnet(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            throw new NotImplementedException();
        }
    }
}
