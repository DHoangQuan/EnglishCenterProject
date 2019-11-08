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
    public class ReportForCustome7daysTeacherController : Controller
    {
        private ModelContext1 db = new ModelContext1();
        public ActionResult IndexReportForCustome7DaysTeacher()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var usingroomin7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            List<ReportForCustome7daysTeacher> datapoint1 = new List<ReportForCustome7daysTeacher>();
            // lecturer in 7days
            var listlecturer = db.People.Where(c => c.Role.Role1 == "Lecturer");
            foreach (var lecturer in listlecturer)
            {
                var teachingslot = 0;
                var readingslot = 0;
                var speakingslot = 0;
                var writingslot = 0;
                var listeningslot = 0;
                foreach (var item in usingroomin7days.Where(p => p.Class.PeopleID == lecturer.PeopleID))
                {
                    teachingslot++;
                }
                foreach(var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Speaking"))
                {
                    speakingslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Writting"))
                {
                    writingslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Listening"))
                {
                    listeningslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Reading"))
                {
                    readingslot++;
                }
                
                float percent = (float)teachingslot / (float)usingroomin7days.Count();
                datapoint1.Add(new ReportForCustome7daysTeacher()
                {
                    LecturerID = lecturer.PeopleID,
                    Name = lecturer.Name,
                    teachingslotin7days = teachingslot,
                    percentofteachingin7days = String.Format("{0:P2}", percent),
                    teachinglistening = listeningslot,
                    teachingreading = readingslot,
                    teachingspeaking = speakingslot,
                    teachingwriting = writingslot,
                });
            }
            return PartialView("../ReportForCustome7daysTeacher/IndexReportForCustome7DaysTeacher",datapoint1);
        }

        public JsonResult IndexReportForCustome7DaysTeacher1234()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var usingroomin7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            List<ReportForCustome7daysTeacher> datapoint1 = new List<ReportForCustome7daysTeacher>();
            // lecturer in 7days
            var listlecturer = db.People.Where(c => c.Role.Role1 == "Lecturer");
            foreach (var lecturer in listlecturer)
            {
                var teachingslot = 0;
                var readingslot = 0;
                var speakingslot = 0;
                var writingslot = 0;
                var listeningslot = 0;
                foreach (var item in usingroomin7days.Where(p => p.Class.PeopleID == lecturer.PeopleID))
                {
                    teachingslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Speaking"))
                {
                    speakingslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Writting"))
                {
                    writingslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Listening"))
                {
                    listeningslot++;
                }
                foreach (var item in usingroomin7days.Where(s => s.Class.PeopleID == lecturer.PeopleID && s.Class.Lesson.Topic.Skill.SkilID == "Reading"))
                {
                    readingslot++;
                }

                float percent = (float)teachingslot / (float)usingroomin7days.Count();
                datapoint1.Add(new ReportForCustome7daysTeacher()
                {
                    LecturerID = lecturer.PeopleID,
                    Name = lecturer.Name,
                    teachingslotin7days = teachingslot,
                    percentofteachingin7days = String.Format("{0:P2}", percent),
                    teachinglistening = listeningslot,
                    teachingreading = readingslot,
                    teachingspeaking = speakingslot,
                    teachingwriting = writingslot,
                });
            }
            return Json(new { datapoint1 }, JsonRequestBehavior.AllowGet);
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