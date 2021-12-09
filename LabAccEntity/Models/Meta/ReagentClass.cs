using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class ReagentClass : MetaBase
    {
        public virtual int? Id { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string FullName { get; set; }
        public virtual int? Order { get; set; }

        public virtual IEqualityComparer<MetaBase> ClassComparer()
        {
            return new ReagentClassComparer();
        }

        public class ReagentClassComparer : IEqualityComparer<MetaBase>
        {
            public bool Equals(MetaBase x, MetaBase y)
            {
                return (
                    (x as ReagentClass).Id == (y as ReagentClass).Id &&
                    (x as ReagentClass).ShortName == (y as ReagentClass).ShortName &&
                    (x as ReagentClass).FullName == (y as ReagentClass).FullName &&
                    (x as ReagentClass).Order == (y as ReagentClass).Order);

            }
            public int GetHashCode(MetaBase obj)
            {
                return (obj as ReagentClass).Id.GetHashCode() * ((obj as ReagentClass).ShortName.GetHashCode() + (obj as ReagentClass).FullName.GetHashCode()) / (obj as ReagentClass).Order.GetHashCode();
            }
        }
    }
}
