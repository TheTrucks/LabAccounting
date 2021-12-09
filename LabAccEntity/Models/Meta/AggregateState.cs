using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class AggregateState : MetaBase
    {
        public virtual int? Id { get; set; }
        public virtual string Description { get; set; }

        public virtual IEqualityComparer<MetaBase> ClassComparer()
        {
            return new AggregateStateComparer();
        }

        public class AggregateStateComparer : IEqualityComparer<MetaBase>
        {
            public bool Equals(MetaBase x, MetaBase y)
            {
                return ((x as AggregateState).Id == (y as AggregateState).Id);
                    
            }
            public int GetHashCode(MetaBase obj)
            {
                return (obj as AggregateState).Id.GetHashCode() * (obj as AggregateState).Description.GetHashCode();
            }
        }
    }
}
