using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl.Contracts
{
    public abstract class ThrottleControlBase
    {
        private int MaxRPS = 10;

        private AutoResetEvent _wait = new AutoResetEvent(false);


        /// <summary>
        /// Increment Counter. if reach MaxRPS, this call will be blocked until the RPS was released.
        /// You can specified timeout to wait until release, if timeout, will return -1.
        /// </summary>
        /// <param name="timeout">give TimeSpan.Zero means wait forever.</param>
        /// <returns>-1 means timeout and do not get lock</returns>
        public abstract int Hit(int millisecondsTimeout);
        //{
        //    if (this._wait.WaitOne(millisecondsTimeout) == true)
        //    {
        //        // enter
        //        return 1;
        //    }
        //    return 0;
        //}

        public async Task<int> HitAsync()
        {
            return -1;
        }


        public abstract int CurrentRPS { get; }


        public WaitHandle GetWorkWait()
        {
            return this._wait;
        }
    }
}
