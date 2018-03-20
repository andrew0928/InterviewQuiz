using Quiz.OnlineTransactions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.OnlineTransactions.Practices
{

    public abstract class RemoteAccount : AccountBase
    {
        protected abstract bool RequireLock(string lockName, int timeout = 30000);
        protected abstract bool ReleaseLock(string lockName);


        public override decimal GetBalance()
        {
            // just call under API
            throw new NotImplementedException();
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            if (this.RequireLock(this.Name) == false) throw new Exception("transaction fail: can not require lock");

            // do transaction

            this.ReleaseLock(this.Name);
            return this.GetBalance();
        }

        public override IEnumerable<Item> GetHistory()
        {
            // just call under API
            throw new NotImplementedException();
        }
    }


    public class LockAccount : AccountBase
    {
        private decimal _balance = 0;
        private List<Item> _history = new List<Item>();
        private object _syncroot = new object();


        public override decimal GetBalance()
        {
            return this._balance;
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            lock (this._syncroot)
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
