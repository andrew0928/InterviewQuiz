using Quiz.LastHourStatistics.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Quiz.LastHourStatistics.Practices
{
    public class InDatabaseEngine : EngineBase
    {
        public InDatabaseEngine(TimeSpan? time_window = null) :
            base(time_window??TimeSpan.FromMinutes(1))
        {

        }

        public override int StatisticResult
        {
            get
            {
                return this.GetSqlConn().ExecuteScalar<int>(
                    @"select sum(amount) from [orders] where time between dateadd(second, @second, getdate()) and getdate();",
                    new { second = this.TimeWindow.TotalSeconds });
            }
        }

        public override int CreateOrders(int amount)
        {
            this.GetSqlConn().ExecuteScalar(@"insert [orders] (amount) values (@amount);", new { amount = amount });
            return amount;
        }

        private SqlConnection GetSqlConn()
        {
            return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AccountDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
