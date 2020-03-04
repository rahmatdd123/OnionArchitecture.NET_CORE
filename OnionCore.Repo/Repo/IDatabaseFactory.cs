using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OnionCore.Repo.Repo
{
    public interface IDatabaseFactory : IDisposable
    {
        void Create();
        IDbTransaction Transaction { get; set; }
        IDbConnection Connection { get; set; }
        IDbCommand CreateCommand(bool useTransaction);
    }       
}
