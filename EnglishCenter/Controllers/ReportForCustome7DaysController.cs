using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishCenter.Models;
using System.Text;

namespace EnglishCenter.Controllers
{
    public class ReportForCustome7DaysController : Controller
    {
        private ModelContext1 db = new ModelContext1();
        public ActionResult IndexReportForCustome7DaysSkill()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var usingroomin7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            // skills and lessons in 7 days
            var listskill = db.Skills;
            List<ReportForCustome7Days> datapoint = new List<ReportForCustome7Days>();
            foreach (var skill in listskill)
            {
                var thenumberofskill = 0;
                var result = usingroomin7days.Where(l => l.Class.Lesson.Topic.Skill.SkilID == skill.SkilID);
                if (result != null)
                {
                    foreach (var numberofskill in usingroomin7days.Where(s => s.Class.Lesson.Topic.Skill.SkilID == skill.SkilID))
                    {
                        thenumberofskill++;
                    }
                    float percent = (float)thenumberofskill / (float)usingroomin7days.Count();
                    datapoint.Add(new ReportForCustome7Days()
                    {
                        SkilID = skill.SkilID,
                        Name = skill.Name,
                        numberofskill = thenumberofskill,
                        percentofskill = String.Format("{0:P2}", percent),
                    });
                }

            }
            return PartialView("../ReportForCustome7Days/IndexReportForCustome7DaysSkill", datapoint);
        }
        public JsonResult IndexReportForCustome7DaysSkill1234()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var usingroomin7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            // skills and lessons in 7 days
            var listskill = db.Skills;
            List<ReportForCustome7Days> datapoint = new List<ReportForCustome7Days>();
            foreach (var skill in listskill)
            {
                var thenumberofskill = 0;
                var result = usingroomin7days.Where(l => l.Class.Lesson.Topic.Skill.SkilID == skill.SkilID);
                if (result != null)
                {
                    foreach (var numberofskill in usingroomin7days.Where(s => s.Class.Lesson.Topic.Skill.SkilID == skill.SkilID))
                    {
                        thenumberofskill++;
                    }
                    float percent = (float)thenumberofskill / (float)usingroomin7days.Count();
                    datapoint.Add(new ReportForCustome7Days()
                    {
                        SkilID = skill.SkilID,
                        Name = skill.Name,
                        numberofskill = thenumberofskill,
                        percentofskill = String.Format("{0:P2}", percent),
                    });
                }

            }
            return Json(new { datapoint }, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}