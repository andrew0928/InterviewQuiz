using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Throttle t = new Throttle();
            int statistic = 0;

            List<Thread> threads = new List<Thread>();
            bool stop = false;
            for (int i = 0; i < 20; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Random rnd = new Random();
                    while (stop == false)
                    {
                        t.Hit(1);
                        Interlocked.Increment(ref statistic);
                        Task.Delay(rnd.Next(200)).Wait();
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            {
                Thread thread = new Thread(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    while (stop == false)
                    {
                        timer.Restart();
                        while (timer.ElapsedMilliseconds < 1000)
                        {
                            t.Hit(1);
                            Interlocked.Increment(ref statistic);
                            Thread.Sleep(0);
                        }
                        Task.Delay(15000).Wait();
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            {
                Thread thread = new Thread(() =>
                {
                    while (stop == false)
                    {
                        Console.WriteLine("{0} per sec", Interlocked.Exchange(ref statistic, 0));
                        Task.Delay(1000).Wait();
                    }
                });
                thread.Start();
                threads.Add(thread);
            }


            Console.ReadLine();
            stop = true;

            foreach(Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
