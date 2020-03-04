using OnionCore.Core.IApplicationService;
using OnionCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Core.Services
{
    public class TestService : ITestService
    {
        public IEnumerable<ListData> lists()
        {
            throw new NotImplementedException();
        }
    }
}
