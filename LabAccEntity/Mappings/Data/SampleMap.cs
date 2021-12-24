using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using LabAccEntity.Models.Meta;
using LabAccEntity.Models.Data;

namespace LabAccEntity.Mappings.Data
{
    public class SampleMap : ClassMap<Sample>
    {
        public SampleMap()
        {
            Table("data.sample");
            Id(x => x.Id).GeneratedBy.Sequence("data.sample_id_seq");
            Map(x => x.Name);
            References(x => x.Category, "category_id");
            References(x => x.AggrState, "aggr_state_id");
            References(x => x.Class, "class_type_id");
            Map(x => x.Precursor);
            Map(x => x.StandartNumber, "standart_number");
            Map(x => x.StandartInfo, "standart_info");
            Map(x => x.BatchNumber, "batch_number");
            Map(x => x.Supplier);
            Map(x => x.Waybill);
            Map(x => x.Contract);
            References(x => x.DefaultUnit, "default_unit_id");
            Map(x => x.DateCreated, "date_created");
            Map(x => x.DateReceived, "date_received");
            Map(x => x.DateWaybill, "date_waybill");
            Map(x => x.DateContract, "date_contract");
            Map(x => x.DateDepleted, "date_depleted");
            Map(x => x.DateExpiration, "date_expiration");
            Map(x => x.DateExpired, "date_expired");

            HasMany(x => x.DataList).Cascade.All();
        }
    }
}
