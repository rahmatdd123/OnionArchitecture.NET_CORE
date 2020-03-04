using OnionCore.Core.Interfaces;
using OnionCore.Core.Models;
using OnionCore.Repo.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OnionCore.Repo.DataRepo
{
    public class DataRepo : IDataRepo
    {
        private DatabaseFactory _databaseFactory;

        public DataRepo(IDatabaseFactory databaseFactory)
        {
            if (databaseFactory == null)
                throw new ArgumentNullException("DatabaseFactory is null");

            _databaseFactory = databaseFactory as DatabaseFactory;

            if (_databaseFactory == null)
            {
                throw new NotSupportedException("DatabaseFactory is null");
            }
        }

        public IEnumerable<ListData> listDatas()
        {
            using (var cmd = _databaseFactory.CreateCommand())
            {
                //SearchBorrowing searchBorrowing = new SearchBorrowing();
                SqlParameter[] parameters = new SqlParameter[0];

                using (var reader = SqlHelper.GetDataReader((SqlConnection)cmd.Connection, "USP_GetAllStatus", (SqlCommand)cmd, CommandType.StoredProcedure, parameters.Cast<SqlParameter>().ToList()))
                {
                    return MappingTodoList(reader).ToList();
                }
            }
        }
        private IEnumerable<ListData> MappingTodoList(IDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    ListData data = new ListData();

                    data.ID = Convert.ToInt32(reader["PK_ID"]);
                    data.Name = reader["Status"].ToString();

                    yield return data;
                }
            }
        }
    }
}
