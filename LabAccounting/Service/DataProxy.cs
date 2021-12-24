using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using NHibernate.Linq;
using LabAccEntity.Models.Meta;
using LabAccEntity.Models.Data;

namespace LabAccounting.Service
{
    internal abstract class CacheCollection<T> where T : MetaBase<T>
    {
        private Tuple<DateTime, List<T>> _cachedItems { get; set; }
        public CacheCollection()
        {
            _cachedItems = new Tuple<DateTime, List<T>>(DateTime.MinValue, new List<T>());
        }
        public virtual ReadOnlyCollection<T> CachedItems 
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _cachedItems.Item1)
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _cachedItems = new Tuple<DateTime, List<T>>(DateTime.UtcNow, session.Query<T>().ToList());
                    }
                }
                return _cachedItems.Item2.AsReadOnly();
            }
        }

        public virtual void SaveNewItem(T Input)
        {
            if (!CachedItems.Contains(Input, Input.ClassComparer()))
            {
                using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                {
                    using (var trans = session.BeginTransaction())
                    {
                        session.Save(Input);
                        trans.Commit();
                    }
                }
                var Tmp = _cachedItems.Item2; Tmp.Add(Input);
                _cachedItems = new Tuple<DateTime, List<T>>(DateTime.UtcNow, Tmp);
            }
        }
    }

    public static class MetaDataProxy
    {
        internal class TemplateCacheCollection : CacheCollection<Template> {    }
        internal class AggrStateCacheCollection : CacheCollection<AggregateState> {    }
        internal class ReagCatCacheCollection : CacheCollection<ReagentCategory> {    }
        internal class ReagClassCacheCollection : CacheCollection<ReagentClass> {    }
        internal class UnitCacheCollection : CacheCollection<Unit> {    }
        internal class ContractTemplateCacheCollection : CacheCollection<ContractTemplate> {    }

        internal static TemplateCacheCollection TemplateCache = new TemplateCacheCollection();
        internal static AggrStateCacheCollection AggrStateCache = new AggrStateCacheCollection();
        internal static ReagCatCacheCollection ReagentCategoryCache = new ReagCatCacheCollection();
        internal static ReagClassCacheCollection ReagentClassCache = new ReagClassCacheCollection();
        internal static UnitCacheCollection UnitCache = new UnitCacheCollection();
        internal static ContractTemplateCacheCollection ContractTemplateCache = new ContractTemplateCacheCollection();

        public static void SaveNewMeta(Template Input) { TemplateCache.SaveNewItem(Input); }
        public static void SaveNewMeta(AggregateState Input) { AggrStateCache.SaveNewItem(Input); }
        public static void SaveNewMeta(ReagentCategory Input) { ReagentCategoryCache.SaveNewItem(Input); }
        public static void SaveNewMeta(ReagentClass Input) { ReagentClassCache.SaveNewItem(Input); }
        public static void SaveNewMeta(Unit Input) { UnitCache.SaveNewItem(Input); }
        public static void SaveNewMeta(ContractTemplate Input) { ContractTemplateCache.SaveNewItem(Input); }
    }

    public static class DynamicDataProxy //caching as well maybe?..
    {
        public static void SaveNewData(Models.SampleAddModel Input, bool WithData)
        {
            SaveNewData(Input.GetSample(WithData));
        }
        public static void SaveNewData(Sample Input)
        {
            using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.Save(Input);
                    trans.Commit();
                }
            }
        }

        public static Tuple<int, List<Sample>> GetSamples(int Page = 1, string Direction = "down", string OrderString = "", bool Full = false)
        {
            if (OrderString == null)
                OrderString = "";
            List<Sample> SampleList = new List<Sample>();

            using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
            {
                SampleList = RetrieveSamples(session, Page, OrderString, Full);

                if (SampleList.Count == 0)
                {
                    Sample LastDate = null;
                    if (Direction == "down")
                    {
                        var TimeLimit = TimeHelper.GetPagedDates(Page).Item1;

                        LastDate = session.Query<Sample>()
                            .Where(SystemTools.GetFilter(OrderString, DateTime.MinValue, TimeLimit))
                            .OrderByDescending(SystemTools.GetOrder(OrderString))
                            .FirstOrDefault(); 
                    }
                    else
                    {
                        var TimeLimit = TimeHelper.GetPagedDates(Page).Item2;

                        LastDate = session.Query<Sample>()
                            .Where(SystemTools.GetFilter(OrderString, TimeLimit, DateTime.UtcNow.AddDays(1)))
                            .OrderBy(SystemTools.GetOrder(OrderString))
                            .FirstOrDefault();
                    }
                    if (LastDate != null)
                    {
                        Page = TimeHelper.CalcPage(SystemTools.GetOrderDate(LastDate, OrderString));
                        SampleList = RetrieveSamples(session, Page, OrderString, Full);
                    }    
                }
            }
            return new Tuple<int, List<Sample>>(Page, SampleList);
        }

        private static List<Sample> RetrieveSamples(NHibernate.ISession session, int Page, string OrderString, bool Full)
        {
            var Times = TimeHelper.GetPagedDates(Page);
            if (Full)
                Times = new Tuple<DateTime, DateTime>(Times.Item1, DateTime.UtcNow.Date);

            return session.Query<Sample>()
                    .Where(SystemTools.GetFilter(OrderString, Times.Item1, Times.Item2))
                    .Fetch(x => x.Category)
                    .Fetch(x => x.AggrState)
                    .Fetch(x => x.Class)
                    .Fetch(x => x.DefaultUnit)
                    .OrderByDescending(SystemTools.GetOrder(OrderString))
                    .ToList();
        }
    }
}