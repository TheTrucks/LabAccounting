using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAccEntity.Models.Meta
{
    public interface MetaBase 
    {
        IEqualityComparer<MetaBase> ClassComparer();
    }
}
