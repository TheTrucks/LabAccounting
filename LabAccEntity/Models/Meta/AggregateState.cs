using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class AggregateState : MetaBase<AggregateState>
    {
        public virtual int? Id { get; set; }
        public virtual string Description { get; set; }

        public virtual IEqualityComparer<AggregateState> ClassComparer()
        {
            return new AggregateStateComparer();
        }

        public class AggregateStateComparer : IEqualityComparer<AggregateState>
        {
            public bool Equals(AggregateState x, AggregateState y)
            {
                return (x.Id == y.Id);
                    
            }
            public int GetHashCode(AggregateState obj)
            {
                return obj.Id.GetHashCode() * obj.Description.GetHashCode();
            }
        }
    }
}
