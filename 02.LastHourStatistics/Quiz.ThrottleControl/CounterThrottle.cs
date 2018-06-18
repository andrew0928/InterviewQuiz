using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public class CounterThrottle : ThrottleBase
    {
        private TimeSpan _timeWindow = TimeSpan.Zero;
        private double _counter = 0;

        public CounterThrottle(double rate, TimeSpan timeWindow) : base(rate)
        {
            this._timeWindow = timeWindow;

            Thread t = new Thread(() =>
            {
                Stopwatch timer = new Stopwatch();
                while (true)
                {
                    this._counter = 0;
                    timer.Restart();
                    SpinWait.SpinUntil(() => { return timer.Elapsed >= this._timeWindow; });
                }
            });
            t.Start();
        }

        public override bool ProcessRequest(int amount, Action exec = null)
        {
            if (amount + this._counter > this._rate_limit * this._timeWindow.TotalSeconds) return false;

            this._counter += amount;
            Task.Run(exec);
            return true;
        }
    }
}
