using System;
using System.Collections.Generic;
using System.Linq;
using LabAccEntity.Models.Data;
using System.Web;

namespace LabAccounting.Service
{
    public static class SystemTools
    {
        public static Func<Sample, DateTime, DateTime, bool> GetFilter(string Input)
        {
            Func<Sample, DateTime, DateTime, bool> Result;
            switch (Input.ToUpperInvariant().Trim())
            {
                case "DATEEXPIRED":
                    Result = (x, l, h) => x.DateExpired > l && x.DateExpired <= h;
                    break;
                case "DATEEXPIRATION":
                    Result = (x, l, h) => x.DateExpiration > l && x.DateExpiration <= h;
                    break;
                case "DATEDEPLETED":
                    Result = (x, l, h) => x.DateDepleted > l && x.DateDepleted <= h;
                    break;
                case "DATEWAYBILL":
                    Result = (x, l, h) => x.DateWaybill > l && x.DateWaybill <= h;
                    break;
                case "DATERECEIVED":
                    Result = (x, l, h) => x.DateReceived > l && x.DateReceived <= h;
                    break;
                case "DATECREATED":
                default:
                    Result = (x, l, h) => x.DateCreated > l && x.DateCreated <= h;
                    break;
            }
            return Result;
        }

        public static Func<Sample, DateTime> GetOrder(string Input)
        {
            Func<Sample, DateTime> Result;
            switch (Input.ToUpperInvariant().Trim())
            {
                case "DATEEXPIRED":
                    Result = x => x.DateExpired;
                    break;
                case "DATEEXPIRATION":
                    Result = x => x.DateExpiration;
                    break;
                case "DATEDEPLETED":
                    Result = x => x.DateDepleted;
                    break;
                case "DATEWAYBILL":
                    Result = x => x.DateWaybill;
                    break;
                case "DATERECEIVED":
                    Result = x => x.DateReceived;
                    break;
                case "DATECREATED":
                default:
                    Result = x => x.DateCreated;
                    break;
            }
            return Result;
        }
    }
}