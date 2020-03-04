using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NET_CORE.Common.Main
{
    public class BLL
    {
        DAL sqlDAL = new DAL();



        public DataTable GetDashboardCount(SortedList sl)
        {
            return sqlDAL.GetData("USP_GetDashboardCount", sl);
        }
    }
}
