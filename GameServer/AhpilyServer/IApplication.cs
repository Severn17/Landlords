namespace AhpilyServer
{
    public interface IApplication
    {
        // 接收数据
        void OnReceive(ClientPeer client, SocketMsg msg);
        // 断开连接
        void OnDisconnet(ClientPeer client);
    }
}
