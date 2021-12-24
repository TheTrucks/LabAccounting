using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public class ContractTemplate : MetaBase<ContractTemplate>
    {
        public virtual int? Id { get; set; }
        public virtual string Contract { get; set; }
        public virtual DateTime DateContract { get; set; }
        public virtual string Waybill { get; set; }
        public virtual DateTime DateWaybill { get; set; }
        public virtual string Supplier { get; set; }
        public virtual DateTime DateReceived { get; set; }

        public virtual IEqualityComparer<ContractTemplate> ClassComparer()
        {
            return new ContractTemplateComparer();
        }

        public class ContractTemplateComparer : IEqualityComparer<ContractTemplate>
        {
            public bool Equals(ContractTemplate x, ContractTemplate y)
            {
                return (
                    x.Contract == y.Contract &&
                    x.DateContract == y.DateContract &&
                    x.Waybill == y.Waybill &&
                    x.DateWaybill == y.DateWaybill &&
                    x.Supplier == y.Supplier &&
                    x.DateReceived == y.DateReceived);

            }
            public int GetHashCode(ContractTemplate obj)
            {
                return (obj.Contract.GetHashCode() + obj.DateContract.GetHashCode() + obj.Waybill.GetHashCode()) / (obj.DateWaybill.GetHashCode() + obj.Supplier.GetHashCode()) - obj.DateReceived.GetHashCode();
            }
        }
    }
}
