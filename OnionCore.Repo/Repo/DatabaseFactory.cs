using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnionCore.Repo.Repo
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private bool _hasConnection;
        public IDbTransaction Transaction { get; set; }
        public IDbConnection Connection { get; set; }

        public DatabaseFactory()
        {
            Create();
        }

        public void Create()
        {
            string connString = "Data Source=RAHMATKU; Initial Catalog=PDSI.MONITORING; UID=sa; Password=password.1;MultipleActiveResultSets=True;Connection Timeout=300;";

            var connection = new SqlConnection(connString);

            connection.Open();

            Connection = connection;

            _hasConnection = true;
        }
        public IDbCommand CreateCommand(bool useTransaction = true)
        {
            var command = Connection.CreateCommand();
            if (useTransaction)
                Transaction = Transaction ?? Connection.BeginTransaction();
            else
                Transaction = null;
            command.Transaction = Transaction;
            return command;
        }

        protected override void DisposeCore()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction = null;
            }

            if (Connection != null && _hasConnection)
            {
                Connection.Close();
                Connection = null;
            }
        }
    }
}
