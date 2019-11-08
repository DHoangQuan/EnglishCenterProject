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
    public class ReportForCustome7DaysLessonsController : Controller
    {
        private ModelContext1 db = new ModelContext1();
        public ActionResult IndexReportForCustome7DaysLessons()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var usingroomin7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            List<ReportForCustome7DaysLessons> datapoint2 = new List<ReportForCustome7DaysLessons>();
            
            // lessons in 7days
            var listlessons = db.Lessons;
            foreach (var lesson in listlessons)
            {
                var repeattimesoflesson = 0;
                var resultlesson = usingroomin7days.Where(l => l.Class.LessonID == lesson.LessonID);
                if (resultlesson != null)
                {
                    foreach (var item in usingroomin7days.Where(l => l.Class.LessonID == lesson.LessonID))
                    {
                        repeattimesoflesson++;
                    }
                    float percent = (float)repeattimesoflesson / (float)usingroomin7days.Count();
                    datapoint2.Add(new ReportForCustome7DaysLessons()
                    {
                        LessonID = lesson.LessonID,
                        LessonName = lesson.LessonName,
                        TopicID = lesson.TopicID,
                        TopicName = lesson.Topic.TopicName,
                        repeattimesoflesson = repeattimesoflesson,
                        percentin7Days = String.Format("{0:P2}", percent),
                    });

                }
            }
            return PartialView("../ReportForCustome7DaysLessons/IndexReportForCustome7DaysLessons",datapoint2);
        }

        public JsonResult IndexReportForCustome7DaysLessons1234()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var usingroomin7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            List<ReportForCustome7DaysLessons> datapoint2 = new List<ReportForCustome7DaysLessons>();

            // lessons in 7days
            var listlessons = db.Lessons;
            foreach (var lesson in listlessons)
            {
                var repeattimesoflesson = 0;
                var resultlesson = usingroomin7days.Where(l => l.Class.LessonID == lesson.LessonID);
                if (resultlesson != null)
                {
                    foreach (var item in usingroomin7days.Where(l => l.Class.LessonID == lesson.LessonID))
                    {
                        repeattimesoflesson++;
                    }
                    float percent = (float)repeattimesoflesson / (float)usingroomin7days.Count();
                    datapoint2.Add(new ReportForCustome7DaysLessons()
                    {
                        LessonID = lesson.LessonID,
                        LessonName = lesson.LessonName,
                        TopicID = lesson.TopicID,
                        TopicName = lesson.Topic.TopicName,
                        repeattimesoflesson = repeattimesoflesson,
                        percentin7Days = String.Format("{0:P2}", percent),
                    });

                }
            }
            return Json(new { datapoint2 }, JsonRequestBehavior.AllowGet);
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