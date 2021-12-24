using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabAccounting.Service
{
    public static class TimeHelper
    {
        public static int DaysPerPage;
        static TimeHelper()
        {
            if (!int.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["DaysPerPage"], out DaysPerPage))
                DaysPerPage = 2;
        }
        public static Tuple<DateTime, DateTime> GetPagedDates(int Page)
        {
            TimeSpan Pager = new TimeSpan(Page * DaysPerPage, 0, 0, 0);
            DateTime LowEnd = DateTime.UtcNow.Date - Pager;
            DateTime HighEnd = LowEnd.AddDays(DaysPerPage);
            if (Page == 1) HighEnd.AddDays(1);

            return new Tuple<DateTime, DateTime>(LowEnd, HighEnd);
        }

        public static int CalcPage(DateTime Date)
        {
            return (int)Math.Ceiling((DateTime.UtcNow.Date - Date.Date).Days / (decimal)DaysPerPage);
        }
    }
}