using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Sample.Data
{
    public abstract class DBHelper
    {
        protected DBHelper()
        {
            
        }

        protected DBHelper(SqlConnectionStringBuilder connString)
        {
            
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public abstract int Execute(string command);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="parameter">参数组</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public abstract int Execute(string command, SQLiteParameter[] parameter);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回第一行第一列值</returns>
        public abstract object ExecuteScalar(string command);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="parmeter">参数组</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public abstract object ExecuteScalar(string command, SQLiteParameter[] parmeter);

        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="commands">SQL语句集合</param>
        public abstract void BatchExecute(IEnumerable<string> commands);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回DataSet数据集</returns>
        public abstract DataSet GetDataSet(string command);

        public abstract DataSet GetDataSet(string command, string tablename);

        public abstract DataSet GetDataSet(string command, out SQLiteCommand sqlitecmd);

        public abstract DataSet GetDataSet(string command, string tablename, out SQLiteCommand sqlitecmd);

        /// <summary>
        /// 更新源数据
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="sqlitecmd">SQL命令</param>
        /// <returns>受影响的行数</returns>
        public abstract int Update(DataSet ds, ref SQLiteCommand sqlitecmd);

        public abstract int Update(DataSet ds, string tablename, ref SQLiteCommand sqlitecmd);
      
    }
}
