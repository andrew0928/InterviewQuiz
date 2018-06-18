using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ThrottleControl
{
    public class DummyThrottle : ThrottleBase
    {
        public DummyThrottle(double rate) : base(rate)
        {
        }

        public override bool ProcessRequest(int amount, Action exec = null)
        {
            //exec?.Invoke();
            Task.Run(exec);
            return true;
        }
    }
}
