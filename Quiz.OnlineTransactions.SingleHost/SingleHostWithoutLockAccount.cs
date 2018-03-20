using Quiz.OnlineTransactions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.OnlineTransactions.SingleHost
{
    public class SingleHostWithoutLockAccount : AccountBase
    {
        private long _balance = 0;


        public override long GetBalance()
        {
            return this._balance;
        }

        public override long Transfer(long transferAmount)
        {
            return this._balance += transferAmount;
        }
    }
}
