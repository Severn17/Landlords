using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AhpilyServer
{
    // 一个需要执行的方法
    public delegate void ExecuteDelegate();
    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExecute
    {
        private static SingleExecute instance = null;

        private static object o = 1;

        public static SingleExecute Instance
        {
            get
            {
                lock (o)
                {
                    if (instance == null)
                    {
                        instance = new SingleExecute();
                    }
                    return instance;
                }
            }
        }


        public Mutex mutex;

        private SingleExecute()
        {
            mutex = new Mutex();
        }

        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="excuteDelegate"></param>
        public void Execute(ExecuteDelegate executeDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();
                executeDelegate();
                mutex.ReleaseMutex();
            }
        }
    }
}
