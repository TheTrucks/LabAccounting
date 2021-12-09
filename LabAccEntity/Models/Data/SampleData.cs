using System;
using LabAccEntity.Models.Meta;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Data
{
    public class SampleData
    {
        public virtual long? Id { get; set; }
        public virtual Sample ParentSample { get; set; }
        public virtual Decimal Quantity { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual string Comment { get; set; }
        public virtual int DepartmentId { get; set; }
        public virtual string Receiver { get; set; }
    }
}
