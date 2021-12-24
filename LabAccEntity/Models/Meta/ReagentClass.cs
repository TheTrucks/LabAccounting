using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class ReagentClass : MetaBase<ReagentClass>
    {
        public virtual int? Id { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string FullName { get; set; }
        public virtual int? Order { get; set; }

        public virtual IEqualityComparer<ReagentClass> ClassComparer()
        {
            return new ReagentClassComparer();
        }

        public class ReagentClassComparer : IEqualityComparer<ReagentClass>
        {
            public bool Equals(ReagentClass x, ReagentClass y)
            {
                return (
                    x.Id == y.Id &&
                    x.ShortName == y.ShortName &&
                    x.FullName == y.FullName &&
                    x.Order == y.Order);

            }
            public int GetHashCode(ReagentClass obj)
            {
                return obj.Id.GetHashCode() * (obj.ShortName.GetHashCode() + obj.FullName.GetHashCode()) / obj.Order.GetHashCode();
            }
        }
    }
}
