using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using LabAccEntity.Models.Meta;

namespace LabAccEntity.Mappings.Meta
{
    public class ContractTemplateMap : ClassMap<ContractTemplate>
    {
        public ContractTemplateMap()
        {
            Table("meta.contract_template");
            Id(x => x.Id).GeneratedBy.Sequence("meta.ctr_template_seq");
            Map(x => x.Contract);
            Map(x => x.DateContract, "date_contract");
            Map(x => x.Waybill);
            Map(x => x.DateWaybill, "date_waybill");
            Map(x => x.Supplier);
            Map(x => x.DateReceived, "date_received");
        }
    }
}
