/// <summary>
/// 网络消息
///  发送的时候，都要发送这个类
/// </summary>
public class SocketMsg
{
    // 操作码
    public int OpCode { get; set; }
    // 子操作
    public int SubCode { get; set; }
    // 参数
    public object Value { get; set; }

    public SocketMsg()
    {

    }
    public SocketMsg(int opCode, int subCode, object value)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }
    public void Change(int opCode,int subCode,object value)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }
}
