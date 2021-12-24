using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class Unit : MetaBase<Unit>
    {
        public virtual int? Id { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string FullName { get; set; }
        public virtual Unit BaseUnit { get; set; }
        public virtual decimal BaseDiff { get; set; }

        public virtual IEqualityComparer<Unit> ClassComparer()
        {
            return new UnitComparer();
        }

        public class UnitComparer : IEqualityComparer<Unit>
        {
            public bool Equals(Unit x, Unit y)
            {
                return (
                    x.Id == y.Id &&
                    x.ShortName == y.ShortName &&
                    x.FullName == y.FullName &&
                    x.BaseUnit.Id == y.BaseUnit.Id &&
                    x.BaseDiff == y.BaseDiff);

            }
            public int GetHashCode(Unit obj)
            {
                return obj.Id.GetHashCode() * (obj.FullName.GetHashCode() + obj.ShortName.GetHashCode() + obj.BaseDiff.GetHashCode()) / obj.BaseUnit.Id.GetHashCode();
            }
        }
    }
}
