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

        public virtual List<T> GetItems(NHibernate.ISession session)
        {
            return session.Query<T>().ToList();
        }
        public virtual T GetItem(T Input)
        {
            return Input;
        }
        public virtual void UpdateItem(NHibernate.ISession session, T Item)
        {
            // do nothing
        }

        public virtual ReadOnlyCollection<T> CachedItems 
        {
            get
            {
                if ((DateTime.UtcNow - TimeSpan.FromMinutes(30)) > _cachedItems.Item1) //todo FromMinutes(30) -> editable from config
                {
                    using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
                    {
                        _cachedItems = new Tuple<DateTime, List<T>>(DateTime.UtcNow, GetItems(session));
                    }
                }
                return _cachedItems.Item2.AsReadOnly();
            }
        }

        public virtual void SaveNewItem(T Input)
        {
            var Compare = Input.ClassComparer();
            var Element = CachedItems.FirstOrDefault(x => Compare.Equals(x, Input));
            using (var session = LabAccEntity.NHibernateHelper.DbConn.SessionFactory.OpenSession())
            {
                if (Element == null)
                {

                    using (var trans = session.BeginTransaction())
                    {
                        session.Save(GetItem(Input));
                        trans.Commit();
                    }

                    var Tmp = _cachedItems.Item2; Tmp.Add(Input);
                    _cachedItems = new Tuple<DateTime, List<T>>(DateTime.UtcNow, Tmp);
                }
                else
                {
                    UpdateItem(session, Element);
                }
            }
        }
    }

    public static class MetaDataProxy
    {
        internal class TemplateCacheCollection : CacheCollection<Template> 
        {
            public override List<Template> GetItems(NHibernate.ISession session)
            {
                return session.Query<Template>().Where(x => x.DateLastUsed > DateTime.UtcNow.Date.AddMonths(-3)).ToList();
            }
            public override Template GetItem(Template Input)
            {
                Input.DateLastUsed = DateTime.UtcNow.Date;
                return Input;
            }
            public override void UpdateItem(NHibernate.ISession session, Template Item)
            {
                var UpdElem = session.Query<Template>().Where(x => x.Id == Item.Id).FirstOrDefault();
                if (UpdElem != null)
                {
                    using (var trans = session.BeginTransaction())
                    {
                        UpdElem.DateLastUsed = DateTime.UtcNow.Date;
                        trans.Commit();
                    }
                }
            }
        }
        internal class ContractTemplateCacheCollection : CacheCollection<ContractTemplate> 
        {
            public override List<ContractTemplate> GetItems(NHibernate.ISession session)
            {
                return session.Query<ContractTemplate>().Where(x => x.DateLastUsed > DateTime.UtcNow.Date.AddMonths(-3)).ToList();
            }
            public override ContractTemplate GetItem(ContractTemplate Input)
            {
                Input.DateLastUsed = DateTime.UtcNow.Date;
                return Input;
            }
            public override void UpdateItem(NHibernate.ISession session, ContractTemplate Item)
            {
                var UpdElem = session.Query<ContractTemplate>().Where(x => x.Id == Item.Id).FirstOrDefault();
                if (UpdElem != null)
                {
                    using (var trans = session.BeginTransaction())
                    {
                        UpdElem.DateLastUsed = DateTime.UtcNow.Date;
                        trans.Commit();
                    }
                }
            }
        }
        internal class AggrStateCacheCollection : CacheCollection<AggregateState> {    }
        internal class ReagCatCacheCollection : CacheCollection<ReagentCategory> {    }
        internal class ReagClassCacheCollection : CacheCollection<ReagentClass> 
        {
            public override List<ReagentClass> GetItems(NHibernate.ISession session)
            {
                return session.Query<ReagentClass>().OrderBy(x => x.Order).ToList();
            }
        }
        internal class UnitCacheCollection : CacheCollection<Unit> {    }

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
                Times = new ValueTuple<DateTime, DateTime>(Times.Item1, DateTime.UtcNow.Date);

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