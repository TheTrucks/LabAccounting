using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabAccEntity.Models.Meta;

namespace LabAccEntity.Models.Data
{
    public class Sample
    {
        public virtual long? Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ReagentCategory Category { get; set; }
        public virtual AggregateState AggrState { get; set; }
        public virtual ReagentClass Class { get; set; }
        public virtual bool Precursor { get; set; }
        public virtual string StandartNumber { get; set; }
        public virtual string StandartInfo { get; set; }
        public virtual string BatchNumber { get; set; }
        public virtual string Supplier { get; set; }
        public virtual string Waybill { get; set; }
        public virtual string Contract { get; set; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateReceived { get; set; }

        public virtual DateTime DateWaybill { get; set; }
        public virtual DateTime DateContract { get; set; }

        public virtual DateTime? DateDepleted { get; set; }

        public virtual DateTime DateExpiration { get; set; }

        public virtual DateTime? DateExpired { get; set; }

        public virtual IList<SampleData> DataList { get; set; }
    }
}
