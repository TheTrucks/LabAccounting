using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabAccEntity.Models.Data;
using LabAccEntity.Models.Meta;
using LabAccounting.Service;
using System.Web.Script.Serialization;

namespace LabAccounting.Controllers
{
    public class JournalController : Controller
    {
        public ActionResult Index(string DateJump)
        {
            int Page = 1;
            string Date = DateTime.UtcNow.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(DateJump))
            {
                DateTime JumpDate;
                if (DateTime.TryParse(DateJump, out JumpDate))
                {
                    Date = JumpDate.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    Page = TimeHelper.CalcPage(JumpDate);
                }
            }
            return View(new Tuple<int, string> (Page, Date));
        }

        [HttpPost]
        public JsonResult GetSampleListJson(int page, string dir, string order)
        {
            using (var session = SessionManager.GetSession)
            {
                var SampleList = DynamicDataProxy.GetSamples(session, page, dir, order);
                var Model = RenderRazorViewToString("_SampleList", SampleList.Item2);
                return Json(new { Page = SampleList.Item1, Samples = Model });
            }
        }

        [HttpPost]
        public PartialViewResult ModalAdd()
        {
            using (var session = SessionManager.GetSession)
            { 
                return PartialView("_AddModal", new Models.SampleAddMetaModel(true, session)); 
            }
        }

        [HttpPost]
        public JsonResult GetTemplatesJson()
        {
            var Serial = new JavaScriptSerializer();
            Serial.RegisterConverters(new JavaScriptConverter[] { new Models.TemplateSerializer(), new Models.ContractTemplateSerializer() });
            using (var session = SessionManager.GetSession)
            {
                var Model = Serial.Serialize(
                    new
                    {
                        NameTemplates = MetaDataProxy.TemplateCache.CachedItems(session),
                        ContractTemplates = MetaDataProxy.ContractTemplateCache.CachedItems(session)
                    });
                return Json(Model);
            }
        }

        [HttpPost]
        public JsonResult SaveTemplate(Models.InputTemplate SaveObj)
        {
            using (var session = SessionManager.GetSession)
            {
                try
                {
                    MetaDataProxy.SaveNewMeta(session, SaveObj.FormTemplate());
                }
                catch (Exception Exc)
                {
                    return Json(new { code = 500, message = Exc.ToString() });
                }
                return Json(new { code = 200, message = "" });
            }
        }

        [HttpPost]
        public JsonResult SaveSample(Models.SampleAddModel Input)
        {
            using (var session = SessionManager.GetSession)
            {
                var ReturnCode = 500;
                var Page = -1;
                try
                {
                    var SampleInput = Input.GetSample(true);
                    DynamicDataProxy.SaveNewData(session, SampleInput);
                    ReturnCode = 275;
                    Page = TimeHelper.CalcPage(SampleInput.DateReceived);
                    MetaDataProxy.SaveNewMeta(session, Input.GetTemplate());
                    ReturnCode = 250;
                    MetaDataProxy.SaveNewMeta(session, Input.GetContractTemplate());
                    ReturnCode = 200;
                }
                catch (Exception Exc)
                {
                    return Json(new { code = ReturnCode, message = Exc.ToString() });
                }
                return Json(new { code = ReturnCode, page = Page, message = "" });
            }
        }

        [HttpPost]
        public JsonResult RemoveSample(long SampleId)
        {
            using (var session = SessionManager.GetSession)
            {
                try
                {
                    DynamicDataProxy.RemoveData(session, SampleId);
                    return Json(new { code = 200 });
                }
                catch (Exception Exc)
                {
                    return Json(new { code = 500, message = Exc.ToString() });
                }
            }
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString().Trim(new char[] { '\r', '\n', ' ' });
            }
        }
    }
}