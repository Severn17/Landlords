namespace AhpilyServer.Util.iTimer
{
    // 当定时器达到事件后的触发
    public delegate void TimerDelegate();
    // 定时器计时
    public class TimerModel
    {
        public int Id;
        public long Time;
        private TimerDelegate timeDelegate;
        public TimerModel(int id, long time, TimerDelegate td)
        {
            this.Id = id;
            this.Time = time;
            this.timeDelegate = td;
        }
        /// <summary>
        /// 触发任务
        /// </summary>
        public void Run()
        {
            timeDelegate();
        }
    }
}
