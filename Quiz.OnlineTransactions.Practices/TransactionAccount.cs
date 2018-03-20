using Quiz.OnlineTransactions.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Quiz.OnlineTransactions.Practices
{

    public class TransactionAccount : AccountBase
    {
        private SqlConnection GetSqlConn()
        {
            return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AccountDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public override decimal GetBalance()
        {
            return (decimal)this.GetSqlConn().ExecuteScalar(@"select [balance] from [accounts] where userid = @param0", this.Name);
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            return 0;
        }

        public override IEnumerable<Item> GetHistory()
        {
            throw new NotImplementedException();
        }
    }
}
