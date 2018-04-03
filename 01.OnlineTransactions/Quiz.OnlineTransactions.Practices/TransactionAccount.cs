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
            return this.GetSqlConn().ExecuteScalar<decimal>(
                @"select [balance] from [accounts] where userid = @name", 
                new { name = this.Name });
        }

        public override decimal ExecTransaction(decimal transferAmount)
        {
            return this.GetSqlConn().ExecuteScalar<decimal>(
                @"
--begin tran
  insert [transactions] ([userid], [amount]) values (@name, @transfer);
  update [accounts] set [balance] = [balance] + @transfer where userid = @name;
  select [balance] from [accounts] where userid = @name;
--commit
",
                new { name = this.Name, transfer = transferAmount });
        }

        public override IEnumerable<TransactionItem> GetHistory()
        {
            throw new NotImplementedException();
        }
    }
}
