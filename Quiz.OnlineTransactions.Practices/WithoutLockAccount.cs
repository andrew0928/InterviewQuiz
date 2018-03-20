using Quiz.OnlineTransactions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.OnlineTransactions.Practices
{
    public class WithoutLockAccount : AccountBase
    {
        private decimal _balance = 0;
        private List<Item> _history = new List<Item>();
        //private object _syncroot = new object();


        public override decimal GetBalance()
        {
            return this._balance;
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            //lock (this._syncroot)
            {
                this._history.Add(new Item()
                {
                    Date = DateTime.Now,
                    Amount = transferAmount,
                    Memo = null
                });
                return this._balance += transferAmount;
            }
        }

        public override IEnumerable<Item> GetHistory()
        {
            return this._history;
        }
    }
}
