using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public class LeakyBucketThrottle : ThrottleBase
    {
        private double _max_bucket = 0;
        private double _current_bucket = 0;
        private object _syncroot = new object();
        private int _interval = 100;    // ms

        private Queue<(int amount, Action exec)> _queue = new Queue<(int amount, Action exec)>();

        public LeakyBucketThrottle(double rate, TimeSpan timeWindow) : base(rate)
        {
            this._max_bucket = rate * timeWindow.TotalSeconds;

            Thread t = new Thread(() =>
            {
                Stopwatch timer = new Stopwatch();
                timer.Restart();


                while (true)
                {
                    timer.Restart();
                    SpinWait.SpinUntil(() => { return timer.ElapsedMilliseconds >= _interval; });


                    double step = this._rate_limit * _interval / 1000;
                    double buffer = 0;

                    lock (this._syncroot)
                    {
                        if (this._current_bucket > 0)
                        {
                            buffer += Math.Min(step, this._current_bucket);
                            this._current_bucket -= buffer;
                            while (this._queue.Count > 0)
                            {
                                var i = this._queue.Peek();
                                if (i.amount > buffer) break;
                                this._queue.Dequeue();
                                buffer -= i.amount;
                                //i.exec?.Invoke();
                                Task.Run(i.exec);
                            }
                        }
                    }
                }
            });
            t.Start();
        }


        public override bool ProcessRequest(int amount, Action exec = null)
        {
            lock (this._syncroot)
            {
                if (this._current_bucket + amount > this._max_bucket) return false;

                this._current_bucket += amount;
                this._queue.Enqueue((amount, exec));
                return true;
            }
        }
    }
}
