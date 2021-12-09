using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LabAccEntity.Models.Meta;
using System.Collections.ObjectModel;

namespace LabAccounting.Models
{
    public class SampleAddMetaModel
    {
        public ReadOnlyCollection<ReagentCategory> Categories;
        public ReadOnlyCollection<AggregateState> AggrStates;
        public ReadOnlyCollection<ReagentClass> Classes;
        public ReadOnlyCollection<Unit> Units;

        public SampleAddMetaModel(bool LoadData)
        {
            if (LoadData)
            {
                Categories = Service.MetaDataProxy.ReagentCategories;
                AggrStates = Service.MetaDataProxy.AggrStates;
                Classes = Service.MetaDataProxy.ReagentClasses;
                Units = Service.MetaDataProxy.Units;
            }
        }
    }

    public class SampleAddModel
    {
        public int Category { get; set; }
        public int AggrState { get; set; }
        public string Name { get; set; }
        public bool Precursor { get; set; }
        public int Class { get; set; }
        public int DefUnit { get; set; }
        public string StdNum { get; set; }
        public string StdInfo { get; set; }
        public decimal Quantity { get; set; }
        public string Comment { get; set; }
        public string Supplier { get; set; }
        public string Waybill { get; set; }
        public string WaybillDate { get; set; }
        public string Batch { get; set; }
        public string ReceivedDate { get; set; }
        public string CreatedDate { get; set; }
        public string ExpirationDate { get; set; }

        public LabAccEntity.Models.Data.Sample GetSample(bool WithData = false)
        {
            LabAccEntity.Models.Data.Sample Result = new LabAccEntity.Models.Data.Sample
            {
                Name = this.Name,
                Category = new ReagentCategory
                {
                    Id = this.Category
                },
                AggrState = new AggregateState
                {
                    Id = this.AggrState
                },
                Class = new ReagentClass
                {
                    Id = this.Class
                },
                Precursor = this.Precursor,
                StandartNumber = this.StdNum,
                StandartInfo = this.StdInfo,
                BatchNumber = this.Batch,
                Supplier = this.Supplier,
                Waybill = this.Waybill,
                DefaultUnit = new Unit
                {
                    Id = this.DefUnit
                },
                DateCreated = DateTime.Parse(this.CreatedDate),
                DateReceived = DateTime.Parse(this.ReceivedDate),
                DateWaybill = DateTime.Parse(this.WaybillDate),
                DateExpiration = DateTime.Parse(this.ExpirationDate)
            };

            if (WithData)
            {
                Result.DataList = new List<LabAccEntity.Models.Data.SampleData>();
                Result.DataList.Add(
                    new LabAccEntity.Models.Data.SampleData // todo check quantity > 0
                    {
                        ParentSample = Result,
                        Quantity = this.Quantity,
                        Unit = new Unit
                        {
                            Id = this.DefUnit
                        },
                        Comment = this.Comment,
                        DepartmentId = AuthorizationModule.Utils.AuthUtil.AuthUser.Department.ID
                    });
            }

            return Result;
        }
    }
}