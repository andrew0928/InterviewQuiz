using Quiz.LastHourStatistics.Contracts;
using Quiz.LastHourStatistics.Practices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.LastHourStatistics.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //FuncTest();
            EngineCompareCheck();
            //LoadTest();
        }


        static void FuncTest()
        {
            EngineBase e =
                //new DummyEngine();
                //new InDatabaseEngine();
                //new InMemoryEngine();
                new InRedisEngine(true);

            bool stop = false;


            // general traffic (oders)
            Task t1 = Task.Run(() =>
            {
                Random rnd = new Random();
                while (stop == false)
                {
                    Task.Delay(1000).Wait();
                    //e.CreateOrders(rnd.Next(10));
                    e.CreateOrders(DateTime.Now.Second);
                }
            });


            int expected_result = (0 + 59) * 60 / 2;
            Task t2 = Task.Run(() => {
                while (stop == false)
                {
                    Task.Delay(300).Wait();
                    Console.WriteLine($"statistic: {e.StatisticResult}, expected: {expected_result}, test: {e.StatisticResult == expected_result}");
                }
            });



            Console.ReadLine();
            stop = true;
            Task.WaitAll(t1, t2);
        }

        static void EngineCompareCheck()
        {
            EngineBase e1 =
                new InMemoryEngine();

            EngineBase e2 =
                new InMemoryEngine();

            bool stop = false;

            int thread_count = 20;

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < thread_count; i++)
            {
                Thread t = new Thread(() =>
                {
                    while (stop == false)
                    {
                        Thread.Sleep(0);
                        e1.CreateOrders(1);
                        e2.CreateOrders(1);
                    }
                });

                threads.Add(t);
                t.Start();
            }
            
            //int expected_result = (0 + 59) * 60 / 2;
            Task t2 = Task.Run(() => {
                while (stop == false)
                {
                    Task.Delay(300).Wait();
                    Console.WriteLine($"statistic #1: {e1.StatisticResult}, statistic #2: {e2.StatisticResult}, compare: {e1.StatisticResult == e2.StatisticResult}");
                }
            });



            Console.ReadLine();
            stop = true;
            //Task.WaitAll(t1, t2);
            foreach (Thread t in threads) t.Join();
            t2.Wait();
        }
    }


}
