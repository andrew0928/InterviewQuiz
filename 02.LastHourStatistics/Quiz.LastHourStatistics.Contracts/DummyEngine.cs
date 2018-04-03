using Quiz.LastHourStatistics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.LastHourStatistics.Contracts
{
    public class DummyEngine : EngineBase
    {
        public override int StatisticResult
        {
            get
            {
                return 0;
            }
        }

        public override int CreateOrders(int amount)
        {
            return 0;
        }
    }
}
