using LabAccEntity.Models.Data;
using LabAccEntity.Models.Meta;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;

namespace LabAccounting.Models
{
    public class TemplateSerializer : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(Template) })); }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Template TmpTemp = obj as Template;

            if (TmpTemp != null)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                result.Add("Id", TmpTemp.Id.Value);
                result.Add("Name", TmpTemp.Name);
                result.Add("Precursor", TmpTemp.Precursor.Value.ToString());
                result.Add("Type", new ReagentClass { Id = TmpTemp.Type.Id.Value });
                result.Add("DefaultUnit", new Unit { Id = TmpTemp.DefaultUnit.Id.Value });
                result.Add("StandartNumber", TmpTemp.StandartNumber);
                result.Add("StandartInfo", TmpTemp.StandartInfo);

                return result;
            }
            return new Dictionary<string, object>();
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return null;
        }
    }

    public class ContractTemplateSerializer : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(ContractTemplate) })); }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            ContractTemplate TmpTemp = obj as ContractTemplate;

            if (TmpTemp != null)
            {
                var CurCulture = CultureInfo.InvariantCulture;
                Dictionary<string, object> result = new Dictionary<string, object>();

                result.Add("Id", TmpTemp.Id.Value);
                result.Add("Contract", TmpTemp.Contract);
                result.Add("DateContract", TmpTemp.DateContract.ToString("dd.MM.yyyy", CurCulture));
                result.Add("Waybill", TmpTemp.Waybill);
                result.Add("DateWaybill", TmpTemp.DateWaybill.ToString("dd.MM.yyyy", CurCulture));
                result.Add("Supplier", TmpTemp.Supplier);
                result.Add("DateReceived", TmpTemp.DateReceived.ToString("dd.MM.yyyy", CurCulture));

                return result;
            }
            return new Dictionary<string, object>();
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return null;
        }
    }

    public class SampleSerializer : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(Sample) })); }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Sample TmpTemp = obj as Sample;

            if (TmpTemp != null)
            {
                var CurCulture = CultureInfo.InvariantCulture;
                Dictionary<string, object> result = new Dictionary<string, object>();

                result.Add("Id", TmpTemp.Id.Value);
                result.Add("Name", TmpTemp.Name);
                result.Add("Precursor", TmpTemp.Precursor.ToString());
                result.Add("Class", TmpTemp.Class);
                result.Add("DefaultUnit", new Unit 
                {
                    Id = TmpTemp.DefaultUnit.Id,
                    BaseUnit = null,
                    BaseDiff = TmpTemp.DefaultUnit.BaseDiff,
                    FullName = TmpTemp.DefaultUnit.FullName,
                    ShortName = TmpTemp.DefaultUnit.ShortName
                });
                result.Add("Category", TmpTemp.Category);
                result.Add("AggrState", TmpTemp.AggrState);
                result.Add("StandartNumber", TmpTemp.StandartNumber);
                result.Add("StandartInfo", TmpTemp.StandartInfo);
                result.Add("BatchNumber", TmpTemp.BatchNumber);
                result.Add("Supplier", TmpTemp.Supplier);
                result.Add("Waybill", TmpTemp.Waybill);
                result.Add("Contract", TmpTemp.Contract);
                result.Add("DateCreated", TmpTemp.DateCreated.ToString("dd.MM.yyyy", CurCulture));
                result.Add("DateReceived", TmpTemp.DateReceived.ToString("dd.MM.yyyy", CurCulture));
                result.Add("DateWaybill", TmpTemp.DateWaybill.ToString("dd.MM.yyyy", CurCulture));
                result.Add("DateContract", TmpTemp.DateContract.ToString("dd.MM.yyyy", CurCulture));
                result.Add("DateDepleted", TmpTemp.DateDepleted.HasValue ? TmpTemp.DateDepleted.Value.ToString("dd.MM.yyyy", CurCulture) : "null");
                result.Add("DateExpiration", TmpTemp.DateExpiration.ToString("dd.MM.yyyy", CurCulture));
                result.Add("DateExpired", TmpTemp.DateExpired.HasValue ? TmpTemp.DateExpired.Value.ToString("dd.MM.yyyy", CurCulture) : "null");
                result.Add("DataList", null);

                return result;
            }
            return new Dictionary<string, object>();
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return null;
        }
    }
}