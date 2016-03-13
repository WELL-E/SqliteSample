using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Sample.Data.DBHepler
 {
    /// <summary>
    /// SQLiteHelper
    /// </summary>
    public class SqliteHelper : DBHelper
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        private readonly SQLiteConnection _strConn;

        private SqliteHelper()
        {
            
        }
        /// <summary>
        /// 初始化 SqliteHelper
        /// </summary>
        public SqliteHelper(SqlConnectionStringBuilder connString)
            : base(connString)
        {
            _strConn = new SQLiteConnection(connString.ConnectionString);
        }


        #region 方法
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public override int Execute(string command)
        {
            var result = -1;
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    result = cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="parameter">参数组</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public override int Execute(string command, SQLiteParameter[] parameter)
        {
            var result = -1;
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddRange(parameter);
                    result = cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回第一行第一列值</returns>
        public override object ExecuteScalar(string command)
        {
            object result = null;
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    result = cmd.ExecuteScalar();
                }
                conn.Close();
            }
         
            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="parmeter">参数组</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public override object ExecuteScalar(string command, SQLiteParameter[] parmeter)
        {
            object result = null;
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddRange(parmeter);
                    result = cmd.ExecuteScalar();
                }
                conn.Close();
            }
           
            return result;
        }

        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="commands">SQL语句集合</param>
        public override void BatchExecute(IEnumerable<string> commands)
        {
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        foreach (var command in commands)
                        {
                            cmd.CommandText = command;
                            cmd.ExecuteScalar();
                        }
                    }
                    trans.Commit();
                }
                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回DataSet数据集</returns>
        public override DataSet GetDataSet(string command)
        {
            return this.GetDataSet(command, string.Empty);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="tablename">表名</param>
        /// <returns>返回记录集</returns>
        public override DataSet GetDataSet(string command, string tablename)
        {
            var ds = new DataSet();

            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var adapter = new SQLiteDataAdapter(command, conn))
                {
                    if (string.Empty.Equals(tablename))
                    {
                        adapter.Fill(ds);
                    }
                    else
                    {
                        adapter.Fill(ds, tablename);
                    }
                }
                conn.Close();
            }

            return ds;
        }

        public override DataSet GetDataSet(string command, out SQLiteCommand sqlitecmd)
        {
            return this.GetDataSet(command, string.Empty, out sqlitecmd);
        }

        public override DataSet GetDataSet(string command, string tablename, out SQLiteCommand sqlitecmd)
        {
            var ds = new DataSet();
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                        sqlitecmd = cmd;
                    }
                }   
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// 更新源数据
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="sqlitecmd">SQL命令</param>
        /// <returns>受影响的行数</returns>
        public override int Update(DataSet ds, ref SQLiteCommand sqlitecmd)
        {
            return this.Update(ds, string.Empty, ref sqlitecmd);
        }

        public override int Update(DataSet ds, string tablename, ref SQLiteCommand sqlitecmd)
        {
            var result = -1;
            using (var conn = new SQLiteConnection(_strConn))
            {
                conn.Open();
                using (var adapter = new SQLiteDataAdapter(sqlitecmd))
                {
                    using (var cmdDuilder = new SQLiteCommandBuilder(adapter))
                    {
                        if (string.Empty.Equals(tablename))
                        {
                            result = adapter.Update(ds);
                        }
                        else
                        {
                            result = adapter.Update(ds, tablename);
                        }
                    }
                }
                conn.Close();
            }
          
            return result;
        }

        #endregion
    }
}
