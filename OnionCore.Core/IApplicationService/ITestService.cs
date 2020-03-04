using OnionCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Core.IApplicationService
{
    public interface ITestService
    {
        IEnumerable<ListData> lists();
    }
}
