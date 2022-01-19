using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using LabAccEntity.Models.Meta;

namespace LabAccEntity.Mappings.Meta
{
    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Table("meta.template");
            Id(x => x.Id).GeneratedBy.Sequence("meta.template_seq");
            Map(x => x.Name);
            Map(x => x.Precursor);
            References(x => x.Type, "class_id");
            References(x => x.DefaultUnit, "default_unit_id");
            Map(x => x.StandartNumber, "std_num");
            Map(x => x.StandartInfo, "std_info");
            Map(x => x.DateLastUsed, "date_used");
        }
    }
}
