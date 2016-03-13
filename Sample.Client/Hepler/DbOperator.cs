using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sample.Data.DBHepler;
using Sample.Model;

namespace Sample.Client.Hepler
{
    public static class DbOperator
    {
        /// <summary>
        /// 获取消息总数
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int GetMessageCount(DBHelper db)
        {
            var result = db.ExecuteScalar("select count(*) from messages");
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 获取指定序号段的消息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IList<MessageModel> GetMessages(DBHelper db, int startIndex, int count)
        {
            if (db == null) return null;

            var sql = String.Format(
               "select * from messages order by rowid desc limit {0}, {1};",
               startIndex, count);

            var ds = db.GetDataSet(sql);
            return (from DataRow row in ds.Tables[0].Rows
                    select new MessageModel
                    {
                        MsgId = row["MsgId"].ToString(),
                        Title = row["Title"].ToString(),
                        Sender = row["Sender"].ToString(),
                        Receiver = row["Receiver"].ToString(),
                        RecvTime = row["RecvTime"].ToString(),
                        Body = row["Body"].ToString(),
                        IsRead = Convert.ToBoolean(row["IsRead"])
                    }).ToList();
        }
    }
}
