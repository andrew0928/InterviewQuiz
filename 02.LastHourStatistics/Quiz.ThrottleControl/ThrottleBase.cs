using Quiz.LastHourStatistics.Contracts;
using Quiz.LastHourStatistics.Practices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public abstract class ThrottleBase
    {
        /// <summary>
        /// 每秒鐘處理量的上限 (平均值)
        /// </summary>
        protected double _rate_limit = 100;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">指定流量上限</param>
        protected ThrottleBase(double rate)
        {
            this._rate_limit = rate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount">Request 的處理量</param>
        /// <param name="exec"></param>
        /// <returns>傳回是否受理 request 的結果。
        /// true: 受理, 會在一定時間內處理該 request;
        /// false: 不受理該 request</returns>
        public abstract bool ProcessRequest(int amount, Action exec = null);
    }







}
