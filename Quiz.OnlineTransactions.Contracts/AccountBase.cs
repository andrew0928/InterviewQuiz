using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.OnlineTransactions.Contracts
{
    public abstract class AccountBase
    {
        public string Name = null;

        /// <summary>
        /// 取得帳戶餘額
        /// </summary>
        /// <returns></returns>
        public abstract decimal GetBalance();

        /// <summary>
        /// 取得所有交易紀錄 (按照順序)
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<TransactionItem> GetHistory();

        /// <summary>
        /// 執行交易，將指定金額轉入該帳戶
        /// </summary>
        /// <param name="transferAmount"></param>
        /// <returns></returns>
        public abstract decimal ExecTransaction(decimal transferAmount);


        ///// <summary>
        ///// 執行多個帳戶之間的轉帳交易。必須確保所有帳戶的交易都是成功的，否則就不應該執行任何一筆交易，或是必須將已執行的交易還原。
        ///// </summary>
        ///// <param name="transes"></param>
        ///// <returns></returns>
        //public static bool ExecTransaction(params TransactionCmd[] transes)
        //{
        //    {
        //        // step 1, 確認所有交易的總轉入轉出為 0
        //        long total = 0;
        //        foreach (TransactionCmd tc in transes) total += tc.amount;
        //        if (total != 0) return false;
        //    }

        //    //
        //    // TODO: 請補上多個帳戶間交易的實作
        //    //
        //    throw new NotImplementedException();
        //}

        //private static void DoTransferUntilSuccess(AccountBase from, long amount)
        //{
        //    while (true)
        //    {
        //        long expectedAmount = from.GetBalance() + amount;
        //        try
        //        {
        //            from.Transfer(amount);
        //            break;
        //        }
        //        catch (NetworkTransferException)
        //        {
        //            if (from.GetBalance() == expectedAmount) break;
        //        }
        //    }
        //    return;
        //}




        public class AccountItem
        {
            //public string Id { get; set; }
            public string Name { get; set; }
            public decimal Balance { get; set; }
        }

        public class TransactionItem
        {
            //public string Id { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Memo { get; set; }
        }
    }



}
