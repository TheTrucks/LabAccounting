using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class Template : MetaBase
    {
        public virtual int? Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool? Precursor { get; set; }
        public virtual ReagentClass Type { get; set; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual string StandartNumber { get; set; }
        public virtual string StandartInfo { get; set; }

        public virtual IEqualityComparer<MetaBase> ClassComparer()
        {
            return new TemplateComparer();
        }

        public class TemplateComparer : IEqualityComparer<MetaBase>
        {
            public bool Equals(MetaBase x, MetaBase y)
            {
                return (
                    (x as Template).Name == (y as Template).Name &&
                    (x as Template).Precursor == (y as Template).Precursor &&
                    (x as Template).Type.Id == (y as Template).Type.Id &&
                    (x as Template).DefaultUnit.Id == (y as Template).DefaultUnit.Id &&
                    (x as Template).StandartInfo == (y as Template).StandartInfo &&
                    (x as Template).StandartNumber == (y as Template).StandartNumber);

            }
            public int GetHashCode(MetaBase obj)
            {
                return (((obj as Template).Name.GetHashCode() + (obj as Template).StandartInfo.GetHashCode() + (obj as Template).StandartNumber.GetHashCode()) / ((obj as Template).Type.Id.GetHashCode() + (obj as Template).DefaultUnit.Id.GetHashCode()) - (obj as Template).Precursor.GetHashCode());
            }
        }
    }
}
