using Quiz.LastHourStatistics.Contracts;
using Quiz.LastHourStatistics.Practices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public class StatisticEngineThrottle : ThrottleBase
    {
        //private EngineBase _peek_engine = null;// new InMemoryEngine(timeWindow: TimeSpan.FromSeconds(1));
        private EngineBase _average_engine = null;// new InMemoryEngine(timeWindow: TimeSpan.FromSeconds(3));
        //private double _peek_limit = 0;


        public StatisticEngineThrottle(double averageRate, TimeSpan averageTimeWindow) : base(averageRate)
        {
            //this._peek_limit = peekRate;

            this._average_engine = new InMemoryEngine(timeWindow: averageTimeWindow);
            //this._peek_engine = new InMemoryEngine(timeWindow: peekTimeWindow);
        }

        public override bool ProcessRequest(int amount, Action exec = null)
        {
            if (
                //this._peek_engine.AverageResult < this._peek_limit &&
                this._average_engine.AverageResult < this._rate_limit)
            {
                //this._peek_engine.CreateOrders(amount);
                this._average_engine.CreateOrders(amount);
                //exec?.Invoke();
                Task.Run(exec);
                return true;
            }

            return false;
        }
    }
}
