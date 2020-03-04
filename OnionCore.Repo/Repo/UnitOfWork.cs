using OnionCore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Repo.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        public void Commit()
        {
            if (databaseFactory.Transaction == null)
            {
                throw new InvalidOperationException(
                    "SQL transaction is null");
            }

            databaseFactory.Transaction.Commit();
            databaseFactory.Transaction = null;
        }
    }
}
