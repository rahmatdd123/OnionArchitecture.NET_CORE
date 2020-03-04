using OnionCore.Core.Interfaces;
using OnionCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Repo.DataRepo
{
    public class TestRepo : ITestRepo
    {
        public IEnumerable<ListData> lists()
        {
            throw new NotImplementedException();
        }
    }
}
