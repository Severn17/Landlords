﻿using System.Collections.Generic;
using System.Net.Sockets;

namespace AhpilyServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class ClientPeer
    {
        public Socket ClientSocket { get; set; }

        public ClientPeer()
        {
            this.ReceiveArgs = new SocketAsyncEventArgs();
            this.ReceiveArgs.UserToken = this;
            this.SendArgs = new SocketAsyncEventArgs();
            this.SendArgs.Completed += SendArgs_Completed;
        }


        #region 接收数据

        public delegate void ReceiveComplted(ClientPeer client, SocketMsg msg);

        // 一个消息完成之后的回调
        public ReceiveComplted receiveCompleted;

        // 接收的异步套接字请求
        public SocketAsyncEventArgs ReceiveArgs { get; set; }
        //是否正在处理请求数据
        private bool isReceiveProcess = false;
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
            if (!isReceiveProcess)
                processReceive();
        }

        private void processReceive()
        {
            isReceiveProcess = true;
            // 解析数据包
            byte[] data = EncodeTool.DeCodePacket(ref dataCache);
            if (data == null)
            {
                isReceiveProcess = false;
                return;
            }
            // 需要再次转成一个具体类型，供我们使用
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
        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnet()
        {
            // 清空数据
            dataCache.Clear();
            isReceiveProcess = false;
            // 给发送数据哪里预留的
            //TODO
            sendQueue.Clear();
            isSeedProcess = false;

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            ClientSocket = null;
        }
        #endregion
        #region 发送数据
        /// <summary>
        /// 发送的消息队列
        /// </summary>
        private Queue<byte[]> sendQueue = new Queue<byte[]>();

        private bool isSeedProcess = false;

        //发送的异步套接字操作
        private SocketAsyncEventArgs SendArgs;
        //发送的时候断开连接的回调
        public delegate void SendDisconnet(ClientPeer client, string reason);

        public SendDisconnet sendDisconnet;
        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作</param>
        /// <param name="value">参数</param>
        public void Send(int opCode, int subCode, object value)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            // 存入消息队列
            sendQueue.Enqueue(packet);
            if (!isSeedProcess)
            {
                send();
            }
        }
        /// <summary>
        /// 处理发送的数据
        /// </summary>
        private void send()
        {
            isSeedProcess = true;
            // 如果数据条数为0 就停止发送
            if (sendQueue.Count == 0)
            {
                isSeedProcess = false;
                return;
            }
            // 取出一条数据
            byte[] packet = sendQueue.Dequeue();
            // 设置消息发送异步对象的发送数据缓冲区
            SendArgs.SetBuffer(packet, 0, packet.Length);
            bool result = ClientSocket.SendAsync(SendArgs);
            if (result == false)
            {
                processSend();
            }
        }
        private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            processSend();
        }
        /// <summary>
        /// 当异步发送请求完成的时候调用
        /// </summary>
        private void processSend()
        {
            // 发送的与没有错误
            if (SendArgs.SocketError != SocketError.Success)
            {
                // 发送错误 客户端断开连接了
                sendDisconnet(this, SendArgs.SocketError.ToString());
            }
            else
            {
                send();
            }
        }


        #endregion
    }
}
