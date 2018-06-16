using Quiz.LastHourStatistics.Contracts;
using Quiz.LastHourStatistics.Practices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public class Throttle
    {
        private EngineBase _peek_engine = new InMemoryEngine(timeWindow: TimeSpan.FromSeconds(1));
        private EngineBase _average_engine = new InMemoryEngine(timeWindow: TimeSpan.FromSeconds(3));

        private int _average_limit = 100;
        private int _peek_limit = 500;


        public void Hit(int amount)
        {
            while (this._peek_engine.AverageResult > this._peek_limit ||
                this._average_engine.AverageResult > this._average_limit)
            {
                Task.Delay(5).Wait();
            }

            this._peek_engine.CreateOrders(amount);
            this._average_engine.CreateOrders(amount);
        }


    }
}
