using Quiz.OnlineTransactions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.OnlineTransactions.Practices
{



    public class LockAccount : AccountBase
    {
        private decimal _balance = 0;
        private List<TransactionItem> _history = new List<TransactionItem>();
        private object _syncroot = new object();


        public override decimal GetBalance()
        {
            return this._balance;
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            lock (this._syncroot)
            {
                this._history.Add(new TransactionItem()
                {
                    Date = DateTime.Now,
                    Amount = transferAmount,
                    Memo = null
                });
                return this._balance += transferAmount;
            }
        }

        public override IEnumerable<TransactionItem> GetHistory()
        {
            return this._history;
        }
    }
}
