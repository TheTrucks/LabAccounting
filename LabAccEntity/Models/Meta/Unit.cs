using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class Unit : MetaBase
    {
        public virtual int? Id { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string FullName { get; set; }
        public virtual Unit BaseUnit { get; set; }
        public virtual decimal BaseDiff { get; set; }

        public virtual IEqualityComparer<MetaBase> ClassComparer()
        {
            return new UnitComparer();
        }

        public class UnitComparer : IEqualityComparer<MetaBase>
        {
            public bool Equals(MetaBase x, MetaBase y)
            {
                return (
                    (x as Unit).Id == (y as Unit).Id &&
                    (x as Unit).ShortName == (y as Unit).ShortName &&
                    (x as Unit).FullName == (y as Unit).FullName &&
                    (x as Unit).BaseUnit.Id == (y as Unit).BaseUnit.Id &&
                    (x as Unit).BaseDiff == (y as Unit).BaseDiff);

            }
            public int GetHashCode(MetaBase obj)
            {
                return (obj as Unit).Id.GetHashCode() * ((obj as Unit).FullName.GetHashCode() + (obj as Unit).ShortName.GetHashCode() + (obj as Unit).BaseDiff.GetHashCode()) / (obj as Unit).BaseUnit.Id.GetHashCode();
            }
        }
    }
}
