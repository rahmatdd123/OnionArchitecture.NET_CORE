using OnionCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Core.Interfaces
{
    public interface IDataRepo
    {
        IEnumerable<ListData> listDatas();
    }
}
