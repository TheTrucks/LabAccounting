using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using LabAccEntity.Models.Meta;
using LabAccEntity.Models.Data;

namespace LabAccounting.Service
{
    public static class MetaDataProxy
    {
        private static readonly Type[] SupportedMetaTypes = new Type[]
        {
            typeof(Template),
            typeof(AggregateState),
            typeof(ReagentCategory),
            typeof(ReagentClass),
            typeof(Unit)
        };

        private static Tuple<DateTime, List<Template>> _templateCache = new Tuple<DateTime, List<Template>>(DateTime.MinValue, new List<Template>());
        private static Tuple<DateTime, List<AggregateState>> _aggrStateCache = new Tuple<DateTime, List<AggregateState>>(DateTime.MinValue, new List<AggregateState>());
        private static Tuple<DateTime, List<ReagentCategory>> _reagentCategoryCache = new Tuple<DateTime, List<ReagentCategory>>(DateTime.MinValue, new List<ReagentCategory>());
        private static Tuple<DateTime, List<ReagentClass>> _reagentClassCache = new Tuple<DateTime, List<ReagentClass>>(DateTime.MinValue, new List<ReagentClass>());
        private static Tuple<DateTime, List<Unit>> _unitCache = new Tuple<DateTime, List<Unit>>(DateTime.MinValue, new List<Unit>());

        public static ReadOnlyCollection<Template> Templates
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _templateCache.Item1)
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _templateCache = new Tuple<DateTime, List<Template>>(DateTime.UtcNow, session.Query<Template>().ToList());
                    }
                }
                return _templateCache.Item2.AsReadOnly();
            }
        }
        public static ReadOnlyCollection<AggregateState> AggrStates
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _aggrStateCache.Item1)
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _aggrStateCache = new Tuple<DateTime, List<AggregateState>>(DateTime.UtcNow, session.Query<AggregateState>().ToList());
                    }
                }
                return _aggrStateCache.Item2.AsReadOnly();
            }
        }
        public static ReadOnlyCollection<ReagentCategory> ReagentCategories
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _reagentCategoryCache.Item1)
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _reagentCategoryCache = new Tuple<DateTime, List<ReagentCategory>>(DateTime.UtcNow, session.Query<ReagentCategory>().ToList());
                    }
                }
                return _reagentCategoryCache.Item2.AsReadOnly();
            }
        }
        public static ReadOnlyCollection<ReagentClass> ReagentClasses
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _reagentClassCache.Item1)
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _reagentClassCache = new Tuple<DateTime, List<ReagentClass>>(DateTime.UtcNow, session.Query<ReagentClass>().OrderBy(x => x.Order).ToList());
                    }
                }
                return _reagentClassCache.Item2.AsReadOnly();
            }
        }
        public static ReadOnlyCollection<Unit> Units
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _unitCache.Item1)
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _unitCache = new Tuple<DateTime, List<Unit>>(DateTime.UtcNow, session.Query<Unit>().ToList());
                    }
                }
                return _unitCache.Item2.AsReadOnly();
            }
        }

        public static void SaveNewMeta(MetaBase Input)
        {
            var CurrentType = Input.GetType();
            if (!SupportedMetaTypes.Contains(CurrentType))
                throw new NotImplementedException($"Saving method for {CurrentType.Name} is not implemented");

            SaveNewMetaData(Input);
        }

        private static void SaveNewMetaData(MetaBase Input)
        {
            var CurrentType = Input.GetType();
            if (CurrentType == typeof(Template))
            {
                if (!Templates.Contains(Input as Template, Input.ClassComparer()))
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        using (var trans = session.BeginTransaction())
                        {
                            session.Save(Input as Template);
                            trans.Commit();
                        }
                    }
                    var Tmp = _templateCache.Item2; Tmp.Add(Input as Template);
                    _templateCache = new Tuple<DateTime, List<Template>>(DateTime.UtcNow, Tmp);
                }
            }
            else if (CurrentType == typeof(ReagentCategory))
            {
                if (!ReagentCategories.Contains(Input as ReagentCategory, Input.ClassComparer()))
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        using (var trans = session.BeginTransaction())
                        {
                            session.Save(Input as ReagentCategory);
                            trans.Commit();
                        }
                    }
                    var Tmp = _reagentCategoryCache.Item2; Tmp.Add(Input as ReagentCategory);
                    _reagentCategoryCache = new Tuple<DateTime, List<ReagentCategory>>(DateTime.UtcNow, Tmp);
                }
            }
            else if (CurrentType == typeof(ReagentClass))
            {
                if (!ReagentClasses.Contains(Input as ReagentClass, Input.ClassComparer()))
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        using (var trans = session.BeginTransaction())
                        {
                            session.Save(Input as ReagentClass);
                            trans.Commit();
                        }
                    }
                    var Tmp = _reagentClassCache.Item2; Tmp.Add(Input as ReagentClass);
                    _reagentClassCache = new Tuple<DateTime, List<ReagentClass>>(DateTime.UtcNow, Tmp);
                }
            }
            else if (CurrentType == typeof(AggregateState))
            {
                if (!AggrStates.Contains(Input as AggregateState, Input.ClassComparer()))
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        using (var trans = session.BeginTransaction())
                        {
                            session.Save(Input as AggregateState);
                            trans.Commit();
                        }
                    }
                    var Tmp = _aggrStateCache.Item2; Tmp.Add(Input as AggregateState);
                    _aggrStateCache = new Tuple<DateTime, List<AggregateState>>(DateTime.UtcNow, Tmp);
                }
            }
            else if (CurrentType == typeof(Unit))
            {
                if (!Units.Contains(Input as Unit, Input.ClassComparer()))
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        using (var trans = session.BeginTransaction())
                        {
                            session.Save(Input as Unit);
                            trans.Commit();
                        }
                    }
                    var Tmp = _unitCache.Item2; Tmp.Add(Input as Unit);
                    _unitCache = new Tuple<DateTime, List<Unit>>(DateTime.UtcNow, Tmp);
                }
            }
        }
    }
}