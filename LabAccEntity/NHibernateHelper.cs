using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SysCon = System.Configuration;

namespace LabAccEntity
{
    public class NHibernateHelper
    {
        private static string GetAmurConnection()
        {
            string ServiceAddr = SysCon.ConfigurationManager.AppSettings["ServiceAddr"];
            string ServiceUser = SysCon.ConfigurationManager.AppSettings["ServiceUser"];
            string ServicePass = SysCon.ConfigurationManager.AppSettings["ServicePass"];
            string ServiceBase = SysCon.ConfigurationManager.AppSettings["ServiceBase"];
            return "Host=" + ServiceAddr + ";Username=" + ServiceUser + ";Password=" + ServicePass + ";Database=" + ServiceBase;
        }

        private static Hibernate.Database _dbconn;
        public static Hibernate.Database DbConn
        {
            get
            {
                if (_dbconn == null || _dbconn.SessionFactory.IsClosed)
                {
                    _dbconn = new Hibernate.Database();
                    _dbconn.Initialize(GetAmurConnection());
                }
                return _dbconn;
            }
        }

        public static void OnStart()
        {
            _dbconn = new Hibernate.Database();
            _dbconn.Initialize(GetAmurConnection());
        }
    }
}
