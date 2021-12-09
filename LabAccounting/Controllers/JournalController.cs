using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabAccEntity.Models.Data;
using LabAccEntity.Models.Meta;
using NHibernate.Linq;
using LabAccounting.Service;
using ConMan = LabAccEntity.NHibernateHelper;
using System.Web.Script.Serialization;

namespace LabAccounting.Controllers
{
    public class JournalController : Controller
    {
        public ActionResult Index(string DateJump)
        {
            int Page = 1;
            if (!string.IsNullOrEmpty(DateJump))
            {
                DateTime JumpDate;
                if (DateTime.TryParse(DateJump, out JumpDate))
                    Page = TimeHelper.CalcPage(JumpDate);
            }
            ViewBag.Page = Page;
            return View();
        }

        public PartialViewResult SampleList(int page, string order)
        {
            return PartialView("_SampleList", GetSamples(page, order));
        }

        [HttpPost]
        public JsonResult GetSampleListJson(int page, string order)
        {
            var Serial = new JavaScriptSerializer();
            Serial.RegisterConverters(new[] { new Models.SampleSerializer() });
            var Model = Serial.Serialize(GetSamples(page, order));
            return Json(Model);
        }

        [HttpPost]
        public JsonResult GetSampleListJsonFull(int page, string order)
        {
            var Serial = new JavaScriptSerializer();
            Serial.RegisterConverters(new[] { new Models.SampleSerializer() });
            var Model = Serial.Serialize(GetSamples(page, order, true));
            return Json(Model);
        }

        [HttpPost]
        public PartialViewResult MetaData()
        {
            return PartialView("_AddModal", new Models.SampleAddMetaModel(true));
        }

        [HttpPost]
        public JsonResult GetTemplatesJson()
        {
            var Serial = new JavaScriptSerializer();
            Serial.RegisterConverters(new[] { new Models.TemplateSerializer() });
            var Model = Serial.Serialize(MetaDataProxy.Templates);
            return Json(Model);
        }

        [HttpPost]
        public JsonResult SaveTemplate(Models.InputTemplate SaveObj)
        {
            try
            {
                MetaDataProxy.SaveNewMeta(SaveObj.FormTemplate());
            }
            catch (Exception Exc)
            {
                return Json(new { code = 500, message = Exc.ToString() });
            }
            return Json(new { code = 200, message = "" });
        }

        [HttpPost]
        public JsonResult SaveSample(Models.SampleAddModel Input)
        {
            try
            {
                using (var session = ConMan.DbConn.SessionFactory.OpenSession())
                {
                    using (var trans = session.BeginTransaction())
                    {
                        session.Save(Input.GetSample(true));
                        trans.Commit();
                    }
                }
            }
            catch (Exception Exc)
            {
                return Json(new { code = 500, message = Exc.ToString() });
            }
            return Json(new { code = 200, message = "" });
        }

        private List<Sample> GetSamples(int Page = 1, string OrderString = "", bool Full = false)
        {
            if (OrderString == null)
                OrderString = "";
            List<Sample> Model = new List<Sample>();
            var Times = TimeHelper.GetPagedDates(Page);
            if (Full)
                Times = new Tuple<DateTime, DateTime>(Times.Item1, DateTime.UtcNow.Date);

            using (var session = ConMan.DbConn.SessionFactory.OpenSession())
            {
                Model = session.Query<Sample>()
                    .Where(SystemTools.GetFilter(OrderString, Times.Item1, Times.Item2))
                    .Fetch(x => x.Category)
                    .Fetch(x => x.AggrState)
                    .Fetch(x => x.Class)
                    .Fetch(x => x.DefaultUnit)
                    .OrderByDescending(SystemTools.GetOrder(OrderString))
                    .ToList();
            }
            return Model;
        }
    }
}