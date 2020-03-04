using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NET_CORE.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NET_CORE.Common.Main
{
    public class DAL
    {
        SqlConnection conn;
        //IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

        ConfigValues configTest = new ConfigValues();
        //String strCon = "Data Source=RAHMATKU; Initial Catalog=PDSI.MONITORING; UID=sa; Password=password.1";
        String strCon = "";
        public DAL()
        {
            strCon = configTest.Get("connectionString");
        }

        public void ExecNonQuery(string sp, SortedList Is)
        {
            if (conn == null)
            {
                conn = new SqlConnection(strCon);
            }
            try
            {
                conn.Open();
                SqlCommand cmn = new SqlCommand(sp, conn);
                cmn.CommandType = CommandType.StoredProcedure;

                foreach (DictionaryEntry entry in Is)
                {
                    string[] temp = entry.Key.ToString().Split('-');
                    if (temp[1].ToLower() == "varchar")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.VarChar, int.Parse(temp[2])).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "int")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Int).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "bigint")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.BigInt).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "date")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Date).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "datetime")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.DateTime).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "bit")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Bit).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "varbinary")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.VarBinary).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "numeric")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Decimal).Value = decimal.Parse(entry.Value.ToString());
                    }
                }
                cmn.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable GetData(string sp, SortedList Is)
        {
            //string test = configuration.GetConnectionString("connectionString");
            DataTable dt = new DataTable("data");
            if (conn == null)
            {
                conn = new SqlConnection(strCon);
            }
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sp, conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                foreach (DictionaryEntry entry in Is)
                {
                    string[] temp = entry.Key.ToString().Split('-');
                    if (temp[1].ToLower() == "varchar")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.VarChar, int.Parse(temp[2])).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "int")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Int).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "bigint")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.BigInt).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "date")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Date).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "datetime")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.DateTime).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "time")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Time).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "bit")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Bit).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "varbinary")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.VarBinary).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "float")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Float).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "numeric")
                    {
                        da.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Decimal).Value = decimal.Parse(entry.Value.ToString());
                    }
                }
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public DataTable GetDataNoSl(string sp)
        {
            DataTable dt = new DataTable("data");
            if (conn == null)
            {
                conn = new SqlConnection(strCon);
            }
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sp, conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public string GetExecScalar(string sp, SortedList Is)
        {
            string strResult;
            if (conn == null)
            {
                conn = new SqlConnection(strCon);
            }
            try
            {
                conn.Open();
                SqlCommand cmn = new SqlCommand(sp, conn);
                cmn.CommandType = CommandType.StoredProcedure;



                foreach (DictionaryEntry entry in Is)
                {
                    string[] temp = entry.Key.ToString().Split('-');
                    if (temp[1].ToLower() == "varchar")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.VarChar, int.Parse(temp[2])).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "int")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Int).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "date")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Date).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "datetime")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.DateTime).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "bit")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Bit).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "varbinary")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.VarBinary).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "numeric")
                    {
                        cmn.Parameters.Add(temp[0].ToString(), SqlDbType.Decimal).Value = decimal.Parse(entry.Value.ToString());
                    }
                }
                object objResult = cmn.ExecuteScalar();
                strResult = objResult.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return strResult;

        }

    }
}
