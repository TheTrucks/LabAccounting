using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class ReagentCategory : MetaBase<ReagentCategory>
    {
        public virtual int? Id { get; set; }
        public virtual string Description { get; set; }
        public virtual IEqualityComparer<ReagentCategory> ClassComparer()
        {
            return new ReagentCategoryComparer();
        }

        public class ReagentCategoryComparer : IEqualityComparer<ReagentCategory>
        {
            public bool Equals(ReagentCategory x, ReagentCategory y)
            {
                return x.Id == y.Id;

            }
            public int GetHashCode(ReagentCategory obj)
            {
                return obj.Id.GetHashCode() / obj.Description.GetHashCode();
            }
        }
    }
}
