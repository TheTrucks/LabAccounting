using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class Unit
    {
        public virtual int? Id { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string FullName { get; set; }
        public virtual Unit BaseUnit { get; set; }
        public virtual decimal BaseDiff { get; set; }
    }
}
