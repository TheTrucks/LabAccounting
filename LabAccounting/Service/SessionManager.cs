using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabAccounting.Service
{
    public static class SessionManager
    {
        private static LabAccEntity.Hibernate.Database _dbConn;

        static SessionManager()
        {
            _dbConn = new LabAccEntity.Hibernate.Database();
            _dbConn.Initialize(System.Web.Configuration.WebConfigurationManager.AppSettings["MainDatabaseConnectionString"]);
        }

        public static NHibernate.ISession GetSession
        {
            get
            {
                if (_dbConn == null || _dbConn.SessionFactory.IsClosed)
                {
                    _dbConn.Drop();
                    _dbConn.Initialize(System.Web.Configuration.WebConfigurationManager.AppSettings["MainDatabaseConnectionString"]);
                }
                return _dbConn.SessionFactory.OpenSession();
            }
        }
    }
}