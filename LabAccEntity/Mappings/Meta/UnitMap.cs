using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using LabAccEntity.Models.Meta;

namespace LabAccEntity.Mappings.Meta
{
    public class UnitMap : ClassMap<Unit>
    {
        public UnitMap()
        {
            Table("meta.unit");
            Id(x => x.Id);
            Map(x => x.ShortName, "short_name");
            Map(x => x.FullName, "full_name");
            References(x => x.BaseUnit, "base_unit_id");
            Map(x => x.BaseDiff, "base_diff");
        }
    }
}
