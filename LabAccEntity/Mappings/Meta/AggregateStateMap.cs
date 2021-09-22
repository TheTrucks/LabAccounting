using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using LabAccEntity.Models.Meta;

namespace LabAccEntity.Mappings.Meta
{
    public class AggregateStateMap : ClassMap<AggregateState>
    {
        public AggregateStateMap()
        {
            Table("meta.aggregate_state");
            Id(x => x.Id);
            Map(x => x.Description);
        }
    }
}
