using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public class TokenBucketThrottle : ThrottleBase
    {
        private double _max_bucket = 0;
        private double _current_bucket = 0;
        private object _syncroot = new object();
        private int _interval = 100;

        public TokenBucketThrottle(double rate, TimeSpan timeWindow) : base(rate)
        {
            this._max_bucket = this._rate_limit * timeWindow.TotalSeconds;

            Thread t = new Thread(() =>
            {
                //int _interval = 100;
                Stopwatch timer = new Stopwatch();
                timer.Restart();


                while (true)
                {
                    timer.Restart();
                    SpinWait.SpinUntil(() => { return timer.ElapsedMilliseconds >= _interval; });


                    double step = this._rate_limit * _interval / 1000;
                    lock (this._syncroot)
                    {
                        this._current_bucket = Math.Min(this._max_bucket, this._current_bucket + step);
                    }
                }
            });
            t.Start();
        }

        public override bool ProcessRequest(int amount, Action exec = null)
        {
            lock (this._syncroot)
            {
                if (this._current_bucket > amount)
                {
                    this._current_bucket -= amount;
                    //exec?.Invoke();
                    Task.Run(exec);
                    return true;
                }
            }
            return false;
        }
    }
}
