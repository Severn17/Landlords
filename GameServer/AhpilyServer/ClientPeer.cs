using System.Collections.Generic;
using System.Net.Sockets;

namespace AhpilyServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    class ClientPeer
    {
        public Socket ClientSocket { get; set; }

        public ClientPeer()
        {
            this.ReceiveArgs = new SocketAsyncEventArgs();
            this.ReceiveArgs.UserToken = this;
        }

        #region 接收数据

        public delegate void ReceiveComplted(ClientPeer client, SocketMsg msg);

        // 一个消息完成之后的回调
        public ReceiveComplted receiveCompleted;

        // 接收的异步套接字请求
        public SocketAsyncEventArgs ReceiveArgs { get; set; }
        //是否正在处理请求数据
        private bool isProcess = false;
        /// <summary>
        /// 一旦接受到数据，就存到缓存区里面
        /// </summary>
        private List<byte> dataCache = new List<byte>();

        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!isProcess)
                processReceive();
        }

        private void processReceive()
        {
            isProcess = true;
            // 解析数据包
            byte[] data = EncodeTool.DeCodePacket(ref dataCache);
            if (data == null)
            {
                isProcess = false;
                return;
            }
            //TODO 需要再次转成一个具体类型，供我们使用
            SocketMsg msg = EncodeTool.DecodeMsg(data);
            //回调给上层
            if (receiveCompleted != null)
            {
                receiveCompleted(this, msg);
            }
            //尾递归
            processReceive();
        }
        #endregion
    }
}
