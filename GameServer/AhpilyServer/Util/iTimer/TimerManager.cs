using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using AhpilyServer.Util.ConCurrent;

namespace AhpilyServer.Util.iTimer
{
    /// <summary>
    /// 计时器管理类
    /// </summary>
    public class TimerManager
    {
        private static TimerManager instance = null;

        public static TimerManager Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                        instance = new TimerManager();
                    return instance;
                }
            }
        }

        // 实现定时器的主要功能
        Timer timer;
        // 存储id 和 模型的映射
        private ConcurrentDictionary<int, TimerModel> idModelDict = new ConcurrentDictionary<int, TimerModel>();

        // 移除的任务id列表
        private List<int> removeList = new List<int>();

        private ConCurrentInt id = new ConCurrentInt(-1);

        public TimerManager()
        {
            timer = new Timer(10);
            timer.Elapsed += Timer_Elapsed;
        }
        /// <summary>
        /// 达到时间间隔触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (removeList)
            {
                TimerModel model = null;
                foreach (var id in removeList)
                {
                    idModelDict.TryRemove(id, out model);
                }
                removeList.Clear();
            }

            foreach (var model in idModelDict.Values)
            {
                if (model.Time <= DateTime.Now.Ticks)
                {
                    model.Run();
                }
            }
        }
        /// <summary>
        ///  添加任务 指定触发时间
        /// </summary>
        /// <param name="datetime">指定时间</param>
        /// <param name="timeDelegate"></param>
        public void AddTimeEvent(DateTime datetime, TimerDelegate timeDelegate)
        {
            long delayTime = datetime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0)
                return;
            AddTimeEvent(delayTime, timeDelegate);
        }
        /// <summary>
        /// 添加任务 指定延迟时间
        /// </summary>
        /// <param name="delayTime">秒</param>
        /// <param name="timeDelegate"></param>
        public void AddTimeEvent(long delayTime, TimerDelegate timeDelegate)
        {
            delayTime = delayTime / 1000;
            TimerModel model = new TimerModel(id.Add_Get(), DateTime.Now.Ticks + delayTime, timeDelegate);
            idModelDict.TryAdd(model.Id, model);
        }
    }
}
