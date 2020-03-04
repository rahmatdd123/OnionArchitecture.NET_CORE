using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Core.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
