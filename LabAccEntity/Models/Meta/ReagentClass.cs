using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class ReagentClass
    {
        public virtual int? Id { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string FullName { get; set; }
    }
}
