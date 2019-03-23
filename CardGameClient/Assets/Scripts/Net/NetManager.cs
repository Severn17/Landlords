using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class NetManager:ManagerBase
{
    public static NetManager instance = null;

    private void Awake()
    {
        instance = this;

        Add(0, this);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
            default:
                break;
        }
    }
    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);

    private void Start()
    {
        client.Connect();
    }
    private void Update()
    {
        if (client == null)
        {
            return;
        }
        while (client.SocketMsgQueue.Count>0)
        {
            SocketMsg msg = client.SocketMsgQueue.Dequeue();
            // TODO 操作这个msg
        }
    }
}
