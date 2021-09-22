using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace LabAccEntity.Hibernate
{
    public class Database
    {
        private ISessionFactory _sessionFactory;

        public ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory;
            }
        }

        public void Initialize(string databaseConnectionString, int batch_size = 1)
        {
            _sessionFactory = Fluently.Configure()
              .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.WrapResultSets, "true"))
              .Database(
                PostgreSQLConfiguration.Standard.ConnectionString(databaseConnectionString))
              //#if DEBUG
              //                .ExposeConfiguration(c => c.SetInterceptor(new SQLDebugOutput()))
              //#endif
              .Mappings(m => m.FluentMappings
              .AddFromAssemblyOf<Models.Data.SampleData>()
              .AddFromAssemblyOf<Models.Meta.AggregateState>()
              .AddFromAssemblyOf<Models.Meta.ReagentCategory>()
              .AddFromAssemblyOf<Models.Meta.ReagentClass>()
              .AddFromAssemblyOf<Models.Meta.Unit>())
              .ExposeConfiguration(cfg => cfg.SetProperty("adonet.batch_size", batch_size.ToString()))
              .BuildSessionFactory();
        }

        public void Drop()
        {
            _sessionFactory.Dispose();
        }

    }
}
