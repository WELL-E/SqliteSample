using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Data.DBHepler;

namespace Sample.Test
{
    [TestClass]
    public class DataProviderTest
    {
        private readonly DBHelper _dbHelper;

        public DataProviderTest()
        {
            var path = @"E:\GitHub\SqliteSample\Sample.Client\SampleData\sample";

            var clientConnStr = new SqlConnectionStringBuilder
            {
                DataSource = path,
                IntegratedSecurity = false,
                MinPoolSize = 1,
                MaxPoolSize = 5,
                ConnectTimeout = 10
            };

            _dbHelper = new SqliteHelper(clientConnStr);
        }

        [TestMethod]
        public void TestInsert()
        {
            var sqls = new List<string>();
            for (var i = 0; i < 1000; i++)
            {
                var sql = String.Format("insert into messages" +
                    "(msgid,title,sender,receiver,body,isread,recvtime) " +
                    "values('{0}'," +
                    "'邮件主题{1}'," +
                    "'testfax01@163.com'," +
                    "'testfax02@163.com'," +
                    "'邮件正文'," +
                    "0," +
                    "'2015-08-12 16:03:00')", Guid.NewGuid().ToString("N"), i);
                sqls.Add(sql);
            }
            _dbHelper.BatchExecute(sqls);
        }

        [TestMethod]
        public void TestDelete()
        {
            _dbHelper.Execute("delete from messages");
        }
    }
}
