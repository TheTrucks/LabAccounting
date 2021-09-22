using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabAccEntity.Models.Data;
using NHibernate.Linq;
using LabAccounting.Service;
using ConMan = LabAccEntity.NHibernateHelper;

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
        public JsonResult SampleListJson(int page, string order)
        {
            return Json(GetSamples(page, order));
        }

        [HttpPost]
        public JsonResult SampleListJsonFull(int page, string order)
        {
            return Json(GetSamples(page, order, true));
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
                    .Where(sample => SystemTools.GetFilter(OrderString)(sample, Times.Item1, Times.Item2))
                    .Fetch(x => x.Category)
                    .Fetch(x => x.AggrState)
                    .Fetch(x => x.Class)
                    .OrderByDescending(SystemTools.GetOrder(OrderString))
                    .ToList();
            }
            return Model;
        }
    }
}