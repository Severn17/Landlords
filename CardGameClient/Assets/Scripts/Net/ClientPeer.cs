using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 客户端socket的封装
/// </summary>
public class ClientPeer
{
    private Socket socket;

    private string ip;
    private int port;

    /// <summary>
    /// 构造socket连接对象
    /// </summary>
    /// <param name="ip">地址</param>
    /// <param name="port">端口号</param>
    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;

        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void Connect()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功");
            startReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #region 接收数据
    //接收的数据缓冲区
    private byte[] receiveBuffer = new byte[1024];

    private List<byte> dataCache = new List<byte>();

    private bool isProcessReceive = false;

    public Queue<SocketMsg> SocketMsgQueue = new Queue<SocketMsg>();

    /// <summary>
    /// 开始异步接收数据
    /// </summary>
    private void startReceive()
    {
        if (socket == null && socket.Connected == false)
        {
            Debug.LogError("没有连接成功没法发送数据");
            return;
        }

        socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, receiveCallBack, socket);

    }
    /// <summary>
    /// 收到消息回调
    /// </summary>
    /// <param name="ar"></param>
    private void receiveCallBack(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tmpByteArray = new byte[length];
            Buffer.BlockCopy(receiveBuffer, 0, tmpByteArray, 0, length);
            // 处理收到的数据
            dataCache.AddRange(tmpByteArray);
            if (isProcessReceive == false)
            {
                processReceive();
            }
            startReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// 处理收到的数据
    /// </summary>
    private void processReceive()
    {
        isProcessReceive = true;

        byte[] data = EncodeTool.DeCodePacket(ref dataCache);

        if (data == null)
        {
            isProcessReceive = false;
            return;
        }

        SocketMsg msg = EncodeTool.DecodeMsg(data);

        SocketMsgQueue.Enqueue(msg);

        Debug.Log(msg.Value);

        // 尾递归
        processReceive();

    }
    #endregion
    #region 发送数据
    public void Send(int opCode, int subCode, object value)
    {
        SocketMsg msg = new SocketMsg(opCode, subCode, value);
        Send(msg);
    }
    public void Send(SocketMsg msg)
    {
        byte[] data = EncodeTool.EncodeMsg(msg);
        byte[] packet = EncodeTool.EncodePacket(data);
        try
        {
            socket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion
}
