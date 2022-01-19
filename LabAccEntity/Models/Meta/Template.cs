using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class Template : MetaBase<Template>
    {
        public virtual int? Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool? Precursor { get; set; }
        public virtual ReagentClass Type { get; set; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual string StandartNumber { get; set; }
        public virtual string StandartInfo { get; set; }
        public virtual DateTime DateLastUsed { get; set; }

        public virtual IEqualityComparer<Template> ClassComparer()
        {
            return new TemplateComparer();
        }

        public class TemplateComparer : IEqualityComparer<Template>
        {
            public bool Equals(Template x, Template y)
            {
                return (
                    x.Name == y.Name &&
                    x.Precursor == y.Precursor &&
                    x.Type.Id == y.Type.Id &&
                    x.DefaultUnit.Id == y.DefaultUnit.Id &&
                    x.StandartInfo == y.StandartInfo &&
                    x.StandartNumber == y.StandartNumber);

            }
            public int GetHashCode(Template obj)
            {
                return (obj.Name.GetHashCode() + obj.StandartInfo.GetHashCode() + obj.StandartNumber.GetHashCode()) / (obj.Type.Id.GetHashCode() + obj.DefaultUnit.Id.GetHashCode()) - obj.Precursor.GetHashCode();
            }
        }
    }
}
