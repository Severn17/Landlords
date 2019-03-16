using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AhpilyServer
{
    // 一个需要执行的方法
    public delegate void ExcuteDelegate();
    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExcute
    {
        public Mutex mutex;

        public SingleExcute()
        {
            mutex = new Mutex();
        }

        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="excuteDelegate"></param>
        public void Excute(ExcuteDelegate excuteDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();
                excuteDelegate();
                mutex.ReleaseMutex();
            }
        }
    }
}
