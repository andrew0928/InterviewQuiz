using Quiz.LastHourStatistics.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.LastHourStatistics.Practices
{
    public class InRedisEngine : EngineBase
    {
        private IDatabase redis = ConnectionMultiplexer.Connect("172.18.253.232:6379").GetDatabase();

        public InRedisEngine(bool start_worker = true, TimeSpan? time_window = null) : base(time_window??TimeSpan.FromMinutes(1))
        {
            if (start_worker)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        Task.Delay(this._interval).Wait();
                        this._statistic_timer_worker();
                    }
                });
            }
        }

        public override int StatisticResult //=> this._statistic_result + this._buffer;
        {
            get
            {
                return (int)this.redis.StringGet("statistic") + (int)this.redis.StringGet("buffer");
            }
        }

        public override int CreateOrders(int amount) //=> Interlocked.Add(ref this._buffer, amount);
        {
            return (int)this.redis.StringIncrement("buffer", amount);
        }

        // 控制統計區間長度
        //private readonly TimeSpan _period = TimeSpan.FromMinutes(1);

        // 控制統計的精確度
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(0.1);

        private void _statistic_timer_worker()
        {
            int buffer_value = (int)this.redis.StringGetSet("buffer", 0);
            this.redis.ListRightPush("queue", QueueItem.Encode(new QueueItem()
            {
                _count = buffer_value,
                _time = DateTime.Now
            }));
            this.redis.StringIncrement("statistic", buffer_value);

            while (true)
            {
                QueueItem dqitem = QueueItem.Decode((string)this.redis.ListGetByIndex("queue", 0));

                if (dqitem._time >= (DateTime.Now - this.TimeWindow)) break;

                dqitem = QueueItem.Decode((string)this.redis.ListLeftPop("queue"));
                this.redis.StringDecrement("statistic", dqitem._count);
            }
        }

        public class QueueItem
        {
            public int _count;
            public DateTime _time;

            public static string Encode(QueueItem value)
            {
                return $"{value._count},{(value._time - DateTime.MinValue).TotalMilliseconds}";
            }

            public static QueueItem Decode(string text)
            {
                string[] segments = text.Split(',');
                return new QueueItem()
                {
                    _count = int.Parse(segments[0]),
                    _time = DateTime.MinValue + TimeSpan.FromMilliseconds(double.Parse(segments[1]))
                };
            }
        }
    }




}
