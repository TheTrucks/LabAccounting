using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using LabAccEntity.Models.Meta;

namespace LabAccEntity.Mappings.Meta
{
    public class ReagentClassMap : ClassMap<ReagentClass>
    {
        public ReagentClassMap()
        {
            Table("meta.reagent_class");
            Id(x => x.Id);
            Map(x => x.ShortName, "short_name");
            Map(x => x.FullName, "full_name");
        }
    }
}
