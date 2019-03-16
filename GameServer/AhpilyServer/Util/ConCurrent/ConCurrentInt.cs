using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer.Util.ConCurrent
{
    /// <summary>
    /// 线程安全int类型
    /// </summary>
    public class ConCurrentInt
    {
        private int value;
        public ConCurrentInt(int value)
        {
            this.value = value;
        }
        /// <summary>
        /// 添加并获取
        /// </summary>
        /// <returns></returns>
        public int Add_Get()
        {
            lock (this)
            {
                value++;
                return value;
            }
        }
        /// <summary>
        /// 减少并获取
        /// </summary>
        /// <returns></returns>
        public int Reduce_Get()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return value;
        }
    }
}
