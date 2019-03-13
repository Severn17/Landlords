using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer
{
    /// <summary>
    /// 客户端的连接池
    ///  重用客户端对象
    /// </summary>
    class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeerQueue;

        public ClientPeerPool(int capacity)
        {
            clientPeerQueue = new Queue<ClientPeer>(capacity);
        }

        public void Enqueue(ClientPeer client)
        {
            clientPeerQueue.Enqueue(client);
        }

        public ClientPeer Dequeue()
        {
            return clientPeerQueue.Dequeue();
        }
    }
}
