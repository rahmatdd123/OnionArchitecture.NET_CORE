using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace OnionCore.Repo.Repo
{
    public static class SqlHelper
    {
        #region " Connection "
        //-----------------------------------
        private const string CONNECTION_STRING_NAME = "DefaultConnectionString";

        private static string mDefaultConnStr = null;

        public static string DefaultConnStr
        {
            get
            {
                return mDefaultConnStr;
            }
            set
            {
                mDefaultConnStr = value;
            }
        }

        static SqlHelper()
        {
            if ("Data Source=RAHMATKU; Initial Catalog=PDSI.MONITORING; UID=sa; Password=password.1;MultipleActiveResultSets=True;Connection Timeout=300;" != null)
            {
                mDefaultConnStr = "Data Source=RAHMATKU; Initial Catalog=PDSI.MONITORING; UID=sa; Password=password.1;MultipleActiveResultSets=True;Connection Timeout=300;";
            }
        }

        // Common DB Object 
        public static SqlConnection GetDBConnection()
        {
            return new SqlConnection(mDefaultConnStr);
        }
        public static SqlConnection GetDBConnection(string ConnStrID)
        {
            if (string.IsNullOrEmpty(ConnStrID))
            {
                throw new Exception("Connection string must be specified or defined.");
            }
            else
            {
                string ConnStr = "Data Source=RAHMATKU; Initial Catalog=PDSI.MONITORING; UID=sa; Password=password.1;MultipleActiveResultSets=True;Connection Timeout=300;";
                if (ConnStr == null)
                    return null;
                else
                    return new SqlConnection(ConnStr);
            }
        }
        //-----------------------------------
        #endregion

        #region " Generate Command and Parameter "
        //-----------------------------------
        private static SqlCommand GetSqlCommand(CommandType CT,
                                                string CmdText,
                                                SqlConnection DBConn,
                                                SqlTransaction DBTrans = null,
                                                int timeout = 0)
        {
            if (DBConn == null && DBTrans != null)
                DBConn = DBTrans.Connection;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CT;
            cmd.CommandText = CmdText;
            cmd.Connection = DBConn;
            cmd.Transaction = DBTrans;
            if (timeout > 0)
                cmd.CommandTimeout = timeout;
            return cmd;
        }
        private static SqlCommand GenerateSqlCommand(SqlCommand cmd,
                                                CommandType CT,
                                               string CmdText,
                                               SqlConnection DBConn,
                                               SqlTransaction DBTrans = null,
                                               int timeout = 0)
        {
            if (DBConn == null && DBTrans != null)
                DBConn = DBTrans.Connection;

            //SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CT;
            cmd.CommandText = CmdText;
            cmd.Connection = DBConn;
            cmd.Transaction = DBTrans;
            if (timeout > 0)
                cmd.CommandTimeout = timeout;
            return cmd;
        }

        public static SqlParameter CreateParameter(string Name, SqlDbType DataType, int Size, object Value)
        {
            return CreateParameter(Name, DataType, Size, Value, ParameterDirection.Input);
        }

        public static SqlParameter CreateParameter(string Name, SqlDbType DataType, int Size, object Value, ParameterDirection Direction)
        {
            return CreateParameter(Name, DataType, Size, Value, Direction, 0);
        }

        public static SqlParameter CreateParameter(string Name, SqlDbType DataType, int Size, object Value, byte Scale)
        {
            return CreateParameter(Name, DataType, Size, Value, ParameterDirection.Input, Scale);
        }

        public static SqlParameter CreateParameter(string Name, SqlDbType DataType, int Size, object Value, ParameterDirection Direction = ParameterDirection.Input, byte Scale = 0)
        {
            SqlParameter oParamater = new SqlParameter();
            oParamater.SqlDbType = DataType;
            oParamater.Direction = Direction;
            oParamater.ParameterName = Name;
            oParamater.Size = Size;
            if (!(Value == null)) oParamater.Value = Value;
            if (!(Scale == 0)) oParamater.Scale = Scale;
            return oParamater;
        }

        private static SqlCommand AddCommandParameters(SqlCommand command, List<SqlParameter> allparameter)
        {
            foreach (SqlParameter param in allparameter)
            {
                command.Parameters.Add(param);
            }
            return command;
        }

        private static SqlParameter[] ObjetToSqlParameter(object dataObject)
        {
            Type type = dataObject.GetType();
            PropertyInfo[] props = type.GetProperties();
            List<SqlParameter> paramList = new List<SqlParameter>();

            for (int i = 0; i < props.Length; i++)
            {

                if (props[i].PropertyType.IsValueType || props[i].PropertyType.Name == "String" || props[i].PropertyType.Name == "Object")
                {
                    object fieldValue = type.InvokeMember(props[i].Name, BindingFlags.GetProperty, null, dataObject, null);
                    SqlParameter sqlParameter = new SqlParameter("@" + props[i].Name, fieldValue);
                    paramList.Add(sqlParameter);
                }
            }
            return paramList.ToArray();
        }
        //-----------------------------------
        #endregion

        #region " DoSqlNonQuery "
        //-----------------------------------
        public static object DoSqlNonQuery(SqlConnection conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters)
        {
            return DoSqlNonQuery(conn, SqlType, cmd, SqlText, parameters, null);
        }

        //public static object DoSqlNonQuery(CommandType SqlType, string SqlText, List<SqlParameter> parameters, SqlTransaction trans)
        //{
        //    return DoSqlNonQuery(null, SqlType, SqlText, parameters, trans);
        //}

        public static object DoSqlNonQuery(SqlConnection conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters, SqlTransaction trans)
        {
            return DoSqlNonQuery(conn, SqlType, cmd, SqlText, parameters, trans, 0);
        }

        public static object DoSqlNonQuery(SqlConnection conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters, SqlTransaction trans, int timeout = 0)
        {
            int result = 0;
            //SqlCommand cmd = new SqlCommand();
            try
            {
                //cmd = GetSqlCommand(SqlType, SqlText, conn, trans, timeout);
                cmd = GenerateSqlCommand(cmd, SqlType, SqlText, conn, trans, timeout);
                if (parameters != null)
                    AddCommandParameters(cmd, parameters);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                result = cmd.ExecuteNonQuery();

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        //-----------------------------------
        #endregion

        #region " PopulateDataTable "
        //-----------------------------------
        public static SqlDataReader GetDataReader(SqlConnection Conn, string SqlText, SqlCommand cmd, CommandType SqlType, List<SqlParameter> parameters)
        {
            return GetDataReader(Conn, SqlType, cmd, SqlText, parameters, 0);
        }

        public static SqlDataReader GetDataReader(SqlConnection Conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters, int timeout = 0)
        {
            //SqlCommand cmd = new SqlCommand();
            try
            {
                if (cmd == null)
                    cmd = GetSqlCommand(SqlType, SqlText, Conn, null, timeout);
                else
                    cmd = GenerateSqlCommand(cmd, SqlType, SqlText, Conn, cmd.Transaction, timeout);
                if (parameters != null)
                    AddCommandParameters(cmd, parameters);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                return cmd.ExecuteReader();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public static DataTable PopulateDataTable(SqlConnection Conn, CommandType SqlType, string SqlText, List<SqlParameter> parameters)
        {
            return PopulateDataTable(Conn, SqlType, SqlText, parameters, 0);
        }

        public static DataTable PopulateDataTable(SqlConnection Conn, CommandType SqlType, string SqlText, List<SqlParameter> parameters, int timeout = 0)
        {
            DataTable Result = new DataTable();
            SqlDataReader dr = GetDataReader(Conn, SqlType, null, SqlText, parameters, timeout);
            Result.Load(dr);
            return Result;
        }

        public static bool IsDataExist(CommandType SqlType, string SqlText, List<SqlParameter> parameters, SqlConnection Conn)
        {
            return IsDataExist(SqlType, SqlText, parameters, Conn, 0);
        }

        public static bool IsDataExist(CommandType SqlType, string SqlText, List<SqlParameter> parameters, SqlConnection Conn, int timeout = 0)
        {
            DataTable dt = PopulateDataTable(Conn, SqlType, SqlText, parameters, timeout);
            return (dt.Rows.Count > 0);
        }

        public static DataTable PopulateDataTable(SqlConnection Conn, string TableName)
        {
            return PopulateDataTable(Conn, TableName, "*", "");
        }

        public static DataTable PopulateDataTable(SqlConnection Conn, string TableName, string clause = "")
        {
            return PopulateDataTable(Conn, TableName, "*", clause, 0);
        }

        public static DataTable PopulateDataTable(SqlConnection Conn, string TableName, string ColumnName = "*", string clause = "")
        {
            return PopulateDataTable(Conn, TableName, ColumnName, clause, 0);
        }

        public static DataTable PopulateDataTable(SqlConnection Conn, string TableName, string ColumnName = "*", string clause = "", int timeout = 0)
        {
            DataTable result = new DataTable();

            //string SqlTxt = "SELECT * FROM [" + TableName + "] ";
            //if (clause != "")
            //    SqlTxt += "WHERE " + clause;

            string SqlTxt = "SELECT ";
            SqlTxt += ((ColumnName + " FROM [") + TableName) + "] ";
            if (clause != "")
                SqlTxt += "WHERE " + clause;

            try
            {
                result = PopulateDataTable(Conn, CommandType.Text, SqlTxt, null, timeout);
            }
            catch
            {
                throw;
            }
            finally
            {

            }
            return result;
        }

        public static bool IsDataExist(SqlConnection Conn, string TableName)
        {
            return IsDataExist(Conn, TableName, "");
        }

        public static bool IsDataExist(SqlConnection Conn, string TableName, string clause = "")
        {
            return IsDataExist(Conn, TableName, clause, 0);
        }

        public static bool IsDataExist(SqlConnection Conn, string TableName, string clause = "", int timeout = 0)
        {
            DataTable dt = PopulateDataTable(Conn, TableName, "*", clause, timeout);
            return (dt.Rows.Count > 0);
        }

        //-----------------------------------
        #endregion

        #region " PopulateDataSet "
        //-----------------------------------
        public static DataSet PopulateDataSet(SqlConnection Conn, string TableName)
        {
            return PopulateDataSet(Conn, TableName, "", 0);
        }

        public static DataSet PopulateDataSet(SqlConnection Conn, string TableName, string clause = "")
        {
            return PopulateDataSet(Conn, TableName, clause, 0);
        }

        public static DataSet PopulateDataSet(SqlConnection Conn, string TableName, string clause = "", int timeout = 0)
        {
            DataSet result = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter();
            //SqlCommand SelectCmd = new SqlCommand();

            string SqlTxt = "SELECT * FROM [" + TableName + "] ";
            if (clause != "")
                SqlTxt += "WHERE " + clause;

            try
            {
                //SelectCmd = GetSqlCommand(CommandType.Text, SqlTxt, Conn, null, timeout);
                //da = new SqlDataAdapter(SelectCmd);
                //da.Fill(result);
                result = PopulateDataSet(Conn, CommandType.Text, SqlTxt, null, timeout);
            }
            catch
            {
                throw;
            }
            finally
            {
                //SelectCmd.Dispose();
            }
            return result;
        }

        public static DataSet PopulateDataSet(SqlConnection Conn, CommandType SqlType, string SqlText, List<SqlParameter> parameters)
        {
            return PopulateDataSet(Conn, SqlType, SqlText, parameters, 0);
        }

        public static DataSet PopulateDataSet(SqlConnection Conn, CommandType SqlType, string SqlText, List<SqlParameter> parameters, int timeout = 0)
        {
            SqlCommand SelectCmd = new SqlCommand();
            try
            {
                DataSet ds = new DataSet();
                SelectCmd = GetSqlCommand(SqlType, SqlText, Conn, null, timeout);
                if (parameters != null)
                    AddCommandParameters(SelectCmd, parameters);
                if (SelectCmd.Connection.State != ConnectionState.Open)
                    SelectCmd.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(SelectCmd);
                da.Fill(ds);

                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                SelectCmd.Dispose();
            }
        }
        //-----------------------------------
        #endregion

        #region " DoSqlAggregateQuery "
        //-----------------------------------
        public static object DoSqlAggregateQuery(SqlConnection conn, string TableName, string ColumnName, string AggSQLFunc)
        {
            return DoSqlAggregateQuery(conn, TableName, ColumnName, AggSQLFunc, 0);
        }

        public static object DoSqlAggregateQuery(SqlConnection conn, string TableName, string ColumnName, string AggSQLFunc, int timeout = 0)
        {
            string SqlTxt = string.Format("SELECT {0} ({1})  as Result FROM [{2}]", AggSQLFunc, ColumnName, TableName);

            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = GetSqlCommand(CommandType.Text, SqlTxt, conn, null, timeout);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                object result = cmd.ExecuteScalar();
                if (result is DBNull)
                {
                    return 0;
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public static object DoSqlAggregateQuery(SqlConnection conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters)
        {
            return DoSqlAggregateQuery(conn, SqlType, cmd, SqlText, parameters, null);
        }

        public static object DoSqlAggregateQuery(SqlConnection conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters, SqlTransaction trans)
        {
            return DoSqlAggregateQuery(conn, SqlType, cmd, SqlText, parameters, trans, 0);
        }

        public static object DoSqlAggregateQuery(SqlConnection conn, CommandType SqlType, SqlCommand cmd, string SqlText, List<SqlParameter> parameters, SqlTransaction trans, int timeout = 0)
        {
            //SqlCommand cmd = new SqlCommand();
            try
            {
                //cmd = GetSqlCommand(SqlType, SqlText, conn, trans, timeout);  //cmd = GetSqlCommand(CommandType.Text, SqlText, conn, null, timeout);
                cmd = GenerateSqlCommand(cmd, SqlType, SqlText, conn, trans, timeout);
                if (parameters != null)
                    AddCommandParameters(cmd, parameters);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                object result = cmd.ExecuteScalar();

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        //-----------------------------------
        #endregion

        #region " GetColumnValues "
        //-----------------------------------
        public static List<object> GetColumnValues(SqlConnection Conn,
                                                            string TableName,
                                                            string ColumnName)
        {
            return GetColumnValues(Conn, TableName, ColumnName, false, "", "", 0);
        }

        public static List<object> GetColumnValues(SqlConnection Conn,
                                                            string TableName,
                                                            string ColumnName,
                                                            bool isDistinct = false)
        {
            return GetColumnValues(Conn, TableName, ColumnName, isDistinct, "", "", 0);
        }

        public static List<object> GetColumnValues(SqlConnection Conn,
                                                            string TableName,
                                                            string ColumnName,
                                                            bool isDistinct = false,
                                                            string WhereCondition = "")
        {
            return GetColumnValues(Conn, TableName, ColumnName, isDistinct, WhereCondition, "", 0);
        }

        public static List<object> GetColumnValues(SqlConnection Conn,
                                                            string TableName,
                                                            string ColumnName,
                                                            bool isDistinct = false,
                                                            string WhereCondition = "",
                                                            string OrderColumn = "")
        {
            return GetColumnValues(Conn, TableName, ColumnName, isDistinct, WhereCondition, OrderColumn, 0);
        }

        public static List<object> GetColumnValues(SqlConnection Conn,
                                                            string TableName,
                                                            string ColumnName,
                                                            bool isDistinct = false,
                                                            string WhereCondition = "",
                                                            string OrderColumn = "",
                                                            int timeout = 0)
        {
            string SqlTxt = "SELECT";
            if (isDistinct)
                SqlTxt += " DISTINCT ";
            SqlTxt += ((ColumnName + " FROM [") + TableName) + "]";
            if (WhereCondition != "")
                SqlTxt += " WHERE " + WhereCondition;
            if (OrderColumn != "")
                SqlTxt += " ORDER BY [" + OrderColumn + "]";

            List<object> alStr = new List<object>();

            SqlDataReader dr = GetDataReader(Conn, CommandType.Text, null, SqlTxt, null, timeout);
            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        alStr.Add(dr[0]);
                    }
                }
                dr.Close();
            }

            return alStr;
        }
        //-----------------------------------
        #endregion

        #region " Bulk Processing "
        //-----------------------------------
        public static bool SaveBulkDataTableToDB(string TableName, DataTable SourceTable, SqlConnection DestConn)
        {
            return SaveBulkDataTableToDB(TableName, SourceTable, -1, DestConn);
        }

        public static bool SaveBulkDataTableToDB(string TableName, DataTable SourceTable, int BulkCopyTimeout, SqlConnection DestConn)
        {
            try
            {
                if (DestConn.State != ConnectionState.Open)
                    DestConn.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(DestConn))
                {
                    s.DestinationTableName = TableName;
                    //s.BatchSize = SourceTable.Rows.Count;
                    //s.NotifyAfter = 10000;
                    if (BulkCopyTimeout != -1)
                        s.BulkCopyTimeout = BulkCopyTimeout;
                    s.WriteToServer(SourceTable);
                    s.Close();
                }
                DestConn.Close();
                return true;
            }
            catch //(Exception e)
            {
                return false;
            }
        }
        //-----------------------------------
        #endregion

        #region " Remarks "
        //-----------------------------------
        /* 
        //REM BECAUSE THIS FUNCTION NOT GENERAL 
        public static List<object> GetPKTable(string TableName, SqlConnection conn)
        {
            string SqlTxt = "sp_pkeys";
            List<SqlParameter> alParams = new List<SqlParameter>();
            List<object> alStr = new List<object>();
            alParams.Add(new SqlParameter("@table_name", TableName));
            
            SqlDataReader dr = DoSqlQuery(CommandType.StoredProcedure, SqlTxt, alParams, conn);
            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        alStr.Add("[" + dr["COLUMN_NAME"].ToString() + "]");
                    }
                }
                dr.Close();
            }
            
            return alStr;
        }

        //REM BECAUSE THIS FUNCTION NOT GENERAL 
        public static bool IsColumnNullable(string TableName, string ColumnName, SqlConnection conn)
        {
            string SqlTxt = "sp_columns";
            bool isNullable = false;
            List<SqlParameter> alParams = new List<SqlParameter>();
            alParams.Add(new SqlParameter("@table_name", TableName));
            alParams.Add(new SqlParameter("@column_name", ColumnName));
            
            SqlDataReader dr = DoSqlQuery(CommandType.StoredProcedure, SqlTxt, alParams, conn);
            if (dr != null)
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    int nullable = Convert.ToInt16(dr["NULLABLE"]);
                    if (nullable == 0)
                    {
                        isNullable = false;
                    }
                    else
                    {
                        isNullable = true;
                    }
                }
                dr.Close();
            }
            
            return isNullable;
        }
        */
        //-----------------------------------
        #endregion

    }
}
