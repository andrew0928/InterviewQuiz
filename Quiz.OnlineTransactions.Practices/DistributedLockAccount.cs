using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Quiz.OnlineTransactions.Contracts;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.OnlineTransactions.Practices
{
    public class DistributedLockAccount : AccountBase, IDisposable
    {
        private RedLockFactory _redlock = null;

        private MongoCollection<AccountMongoEntity> _accounts = null;
        private MongoCollection<TransactionMongoEntity> _transactions = null;

        public DistributedLockAccount() : base()
        {
            MongoClient mclient = new MongoClient("mongodb://172.18.248.6:27017");
            MongoServer mserver = mclient.GetServer();
            MongoDatabase mongoDB = mserver.GetDatabase("bank");

            this._accounts = mongoDB.GetCollection<AccountMongoEntity>("accounts");
            this._transactions = mongoDB.GetCollection<TransactionMongoEntity>("transactions");

            this._redlock = RedLockFactory.Create(new List<RedLockEndPoint>() { new DnsEndPoint("172.18.254.68", 6379) });
        }


        public override decimal GetBalance()
        {
            var acc = this._accounts.FindOne(Query.EQ("Name", this.Name));
            return (acc == null)?(0):(acc.Balance);
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            var resource = $"account-transaction::{this.Name}";
            var expiry = TimeSpan.FromSeconds(5);
            var wait = TimeSpan.FromSeconds(5);
            var retry = TimeSpan.FromMilliseconds(50);

            using (var redLock = this._redlock.CreateLock(resource, expiry, wait, retry))
            {
                if (redLock.IsAcquired)
                {
                    AccountMongoEntity acc = this._accounts.FindOne(Query.EQ("Name", this.Name));
                    if (acc == null)
                    {
                        this._accounts.Insert(acc = new AccountMongoEntity()
                        {
                            _id = ObjectId.GenerateNewId(),
                            Name = this.Name,
                            Balance = transferAmount
                        });
                    }
                    else
                    {
                        acc.Balance += transferAmount;
                        this._accounts.Save(acc);
                    }

                    this._transactions.Insert(new TransactionMongoEntity()
                    {
                        _id = ObjectId.GenerateNewId(),
                        Date = DateTime.Now,
                        Amount = transferAmount
                    });

                    return acc.Balance;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public override IEnumerable<TransactionItem> GetHistory()
        {
            // just call under API
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public class TransactionMongoEntity : TransactionItem
        {
            public ObjectId _id { get; set; }
        }

        public class AccountMongoEntity : AccountItem
        {
            public ObjectId _id { get; set; }
        }
    }

}
