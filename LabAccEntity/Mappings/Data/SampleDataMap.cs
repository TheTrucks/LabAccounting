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
    public class SampleDataMap : ClassMap<SampleData>
    {
        public SampleDataMap()
        {
            Table("data.sample_data");
            Id(x => x.Id).GeneratedBy.Sequence("data.sample_data_id_seq");
            References(x => x.ParentSample, "sample_id");
            Map(x => x.Quantity);
            References(x => x.Unit, "unit_id");
            Map(x => x.Comment);
            Map(x => x.DepartmentId, "department_id");
        }
    }
}
