using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LabAccEntity.Models.Data;
using System.Web;

namespace LabAccounting.Service
{
    public static class SystemTools
    {
        public static Expression<Func<Sample, bool>> GetFilter(string Input, DateTime DateLow, DateTime DateHigh)
        {
            Expression<Func<Sample, bool>> Result;
            switch (Input.ToUpperInvariant().Trim())
            {
                case "DATEEXPIRED":
                    Result = x => x.DateExpired > DateLow && x.DateExpired <= DateHigh;
                    break;
                case "DATEEXPIRATION":
                    Result = x => x.DateExpiration > DateLow && x.DateExpiration <= DateHigh;
                    break;
                case "DATEDEPLETED":
                    Result = x => x.DateDepleted > DateLow && x.DateDepleted <= DateHigh;
                    break;
                case "DATEWAYBILL":
                    Result = x => x.DateWaybill > DateLow && x.DateWaybill <= DateHigh;
                    break;
                case "DATECREATED":
                    Result = x => x.DateCreated > DateLow && x.DateCreated <= DateHigh;
                    break;
                case "DATERECEIVED":
                default:
                    Result = x => x.DateReceived > DateLow && x.DateReceived <= DateHigh;
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
                case "DATECREATED":
                    Result = x => x.DateCreated;
                    break;
                case "DATERECEIVED":
                default:
                    Result = x => x.DateReceived;
                    break;
            }
            return Result;
        }
    }
}