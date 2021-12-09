using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class ReagentCategory : MetaBase
    {
        public virtual int? Id { get; set; }
        public virtual string Description { get; set; }
        public virtual IEqualityComparer<MetaBase> ClassComparer()
        {
            return new ReagentCategoryComparer();
        }

        public class ReagentCategoryComparer : IEqualityComparer<MetaBase>
        {
            public bool Equals(MetaBase x, MetaBase y)
            {
                return ((x as ReagentCategory).Id == (y as ReagentCategory).Id);

            }
            public int GetHashCode(MetaBase obj)
            {
                return (obj as ReagentCategory).Id.GetHashCode() / (obj as ReagentCategory).Description.GetHashCode();
            }
        }
    }
}
