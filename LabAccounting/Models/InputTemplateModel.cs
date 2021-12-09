using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabAccounting.Models
{
    public class InputTemplate
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public bool Precursor { get; set; }
        public int DefUnit { get; set; }
        public string StdInfo { get; set; }
        public string StdNumber { get; set; }

        public LabAccEntity.Models.Meta.Template FormTemplate()
        {
            var Result = new LabAccEntity.Models.Meta.Template
            {
                Name = this.Name,
                Precursor = this.Precursor,
                StandartInfo = StdInfo,
                StandartNumber = StdNumber,
                Type = new LabAccEntity.Models.Meta.ReagentClass
                {
                    Id = this.Type
                },
                DefaultUnit = new LabAccEntity.Models.Meta.Unit
                {
                    Id = this.DefUnit
                }
            };

            return Result;
        }
    }
}