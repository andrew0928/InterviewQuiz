using Quiz.LastHourStatistics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.LastHourStatistics.Practices
{
    public class InMemoryEngine : EngineBase
    {
        private bool _use_lock = false;

        public InMemoryEngine(bool use_lock = true)
        {
            this._queue = new Queue<QueueItem>();
            this._use_lock = use_lock;

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(this._interval).Wait();
                    this._statistic_timer_worker();
                }
            });
        }

        public override int StatisticResult => this._statistic_result + this._buffer;

        public override int CreateOrders(int amount)
        {
            if (this._use_lock)
            {
                return Interlocked.Add(ref this._buffer, amount);
            }
            else
            {
                return (this._buffer += amount);
            }
            
        }

        // 控制統計區間長度
        private readonly TimeSpan _period = TimeSpan.FromMinutes(1);

        // 控制統計的精確度
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(0.1);

        private Queue<QueueItem> _queue = null;

        private int _statistic_result = 0;

        private int _buffer = 0;

        private void _statistic_timer_worker()
        {
            int buffer_value = 0;

            if (this._use_lock)
            {
                buffer_value = Interlocked.Exchange(ref this._buffer, 0);
            }
            else
            {
                buffer_value = this._buffer;
                this._buffer = 0;
            }

            this._queue.Enqueue(new QueueItem()
            {
                _count = buffer_value,
                _time = DateTime.Now
            });
            this._statistic_result += buffer_value;

            while (true)
            {
                if (this._queue.Peek()._time >= (DateTime.Now - this._period)) break;
                {
                    QueueItem dqitem = this._queue.Dequeue();
                    this._statistic_result -= dqitem._count;
                }
            }
        }

        public class QueueItem
        {
            public int _count;
            public DateTime _time;
        }
    }




}
