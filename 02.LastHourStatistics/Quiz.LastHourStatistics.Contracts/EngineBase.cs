using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.LastHourStatistics.Contracts
{
    public abstract class EngineBase
    {
        public EngineBase(TimeSpan timeWindow)
        {
            this.TimeWindow = timeWindow;
        }

        public TimeSpan TimeWindow { get; private set; }

        /// <summary>
        /// 新增訂單紀錄
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public abstract int CreateOrders(int amount);

        /// <summary>
        /// 取得 10 sec 內的所有 Order Amount 統計結果
        /// </summary>
        public abstract int StatisticResult { get; }

        public double AverageResult
        {
            get
            {
                return 1.0D * this.StatisticResult / this.TimeWindow.TotalSeconds;
            }
        }
    }
}
