using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishCenter.Models;

namespace EnglishCenter.Controllers
{
    public class StudentTimesController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        // GET: StudentTimes
        public ActionResult Index()
        {
            var studentTimes = db.StudentTimes.Include(s => s.Person);
            return View(studentTimes.ToList());
        }
        public ActionResult PartialIndexInDetails(string id)
        {
            var studentTimes = db.StudentTimes.Where(c => c.StudentID == id);
            return PartialView("../StudentTimes/PartialIndexInDetails", studentTimes);
        }
        public ActionResult ReportStudentTime(int year, string idstudent)
        {
            if (Session["ReportForStudentTimes"] != null)
            {
                List<ReportForStudentTimes> lhnbt = Session["ReportForStudentTimes"] as List<ReportForStudentTimes>;
                lhnbt.Clear();
            }
            Session["ParaForReportStudentTime1Year"] = null;
            Session["ParaForReportStudentTime1Year"] = year;
            //Session["PassIdStudent"] = null;
            //Session["PassIdStudent"] = idstudent;
            Session["ParaForReportStudentTime1Idstudent"] = null;
            Session["ParaForReportStudentTime1Idstudent"] = idstudent;
            Session["StudentTimes1"] = null;
            Session["StudentTimes2"] = null;
            Session["StudentTimes3"] = null;
            Session["StudentTimes4"] = null;
            Session["StudentTimes5"] = null;
            Session["StudentTimes6"] = null;
            Session["StudentTimes7"] = null;
            Session["StudentTimes8"] = null;
            Session["StudentTimes9"] = null;
            Session["StudentTimes10"] = null;
            Session["StudentTimes11"] = null;
            Session["StudentTimes12"] = null;
            Session["TotalStudent"] = null;
            //string idstudent = Session["PersonID"].ToString();
            var getalltime = db.QuantityAndAttendances.Where(c => c.PeopleID == idstudent && c.UsingRoom.Date.Value.Year == year);
            int number = 0;
            for (int i = 1; i <= 12; i++)
            {
                var CountTimesInMonthCheck = getalltime.Where(c => c.UsingRoom.Date.Value.Month == i);
                if (CountTimesInMonthCheck.Count() != 0)
                {
                    number = number + CountTimesInMonthCheck.Count();
                }
            }
            Session["TotalStudent"] = number;
            for (int i = 1; i <= 12; i++)
            {
                var CountTimesInMonthCheck = getalltime.Where(c => c.UsingRoom.Date.Value.Month == i);
                if (i == 1)
                {
                    Session["StudentTimes1"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 2)
                {
                    Session["StudentTimes2"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 3)
                {
                    Session["StudentTimes3"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 4)
                {
                    Session["StudentTimes4"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 5)
                {
                    Session["StudentTimes5"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 6)
                {
                    Session["StudentTimes6"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 7)
                {
                    Session["StudentTimes7"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 8)
                {
                    Session["StudentTimes8"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 9)
                {
                    Session["StudentTimes9"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 10)
                {
                    Session["StudentTimes10"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 11)
                {
                    Session["StudentTimes11"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 12)
                {
                    Session["StudentTimes12"] = CountTimesInMonthCheck.Count();
                }
            }
            string StudentName = Session["DetailsPersonName"].ToString();
            int total = Convert.ToInt32(Session["TotalStudent"].ToString());
            double average = total / 12;
            Session["ReportForStudentTimes"] = new List<ReportForStudentTimes>();
            List<ReportForStudentTimes> rpfstt = Session["ReportForStudentTimes"] as List<ReportForStudentTimes>;
            ReportForStudentTimes newobj = new ReportForStudentTimes()
            {
                PeopleID = idstudent,
                Name = StudentName,
                TotalTime = total,
                Year = year,
                StudentTimesJan = Convert.ToInt32(Session["StudentTimes1"].ToString()),
                StudentTimesFeb = Convert.ToInt32(Session["StudentTimes2"].ToString()),
                StudentTimesMar = Convert.ToInt32(Session["StudentTimes3"].ToString()),
                StudentTimesApr = Convert.ToInt32(Session["StudentTimes4"].ToString()),
                StudentTimesMay = Convert.ToInt32(Session["StudentTimes5"].ToString()),
                StudentTimesJune = Convert.ToInt32(Session["StudentTimes6"].ToString()),
                StudentTimesJuly = Convert.ToInt32(Session["StudentTimes7"].ToString()),
                StudentTimesAug = Convert.ToInt32(Session["StudentTimes8"].ToString()),
                StudentTimesSep = Convert.ToInt32(Session["StudentTimes9"].ToString()),
                StudentTimesOct = Convert.ToInt32(Session["StudentTimes10"].ToString()),
                StudentTimesNov = Convert.ToInt32(Session["StudentTimes11"].ToString()),
                StudentTimesDec = Convert.ToInt32(Session["StudentTimes12"].ToString()),
                StudentAverageTimes = Math.Round(average),
            };
            rpfstt.Add(newobj);
            return PartialView("../StudentTimes/ReportStudentTime", rpfstt);
        }
        public ActionResult ReportStudentTime1(int year)
        {
            Session["ParaForReportStudentTime1Year"] = null;
            Session["ParaForReportStudentTime1Year"] = year;
            if (Session["ReportForStudentTimes"] != null)
            {
                List<ReportForStudentTimes> lhnbt = Session["ReportForStudentTimes"] as List<ReportForStudentTimes>;
                lhnbt.Clear();
            }
            string idstudent = Session["ParaForReportStudentTime1Idstudent"].ToString();
            Session["StudentTimes1"] = null;
            Session["StudentTimes2"] = null;
            Session["StudentTimes3"] = null;
            Session["StudentTimes4"] = null;
            Session["StudentTimes5"] = null;
            Session["StudentTimes6"] = null;
            Session["StudentTimes7"] = null;
            Session["StudentTimes8"] = null;
            Session["StudentTimes9"] = null;
            Session["StudentTimes10"] = null;
            Session["StudentTimes11"] = null;
            Session["StudentTimes12"] = null;
            Session["TotalStudent"] = null;
            //string idstudent = Session["PersonID"].ToString();
            var getalltime = db.QuantityAndAttendances.Where(c => c.PeopleID == idstudent && c.UsingRoom.Date.Value.Year == year);
            int number = 0;
            for (int i = 1; i <= 12; i++)
            {
                var CountTimesInMonthCheck = getalltime.Where(c => c.UsingRoom.Date.Value.Month == i);
                if (CountTimesInMonthCheck.Count() != 0)
                {
                    number = number + CountTimesInMonthCheck.Count();
                }
            }
            Session["TotalStudent"] = number;
            for (int i = 1; i <= 12; i++)
            {
                var CountTimesInMonthCheck = getalltime.Where(c => c.UsingRoom.Date.Value.Month == i);
                if (i == 1)
                {
                    Session["StudentTimes1"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 2)
                {
                    Session["StudentTimes2"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 3)
                {
                    Session["StudentTimes3"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 4)
                {
                    Session["StudentTimes4"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 5)
                {
                    Session["StudentTimes5"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 6)
                {
                    Session["StudentTimes6"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 7)
                {
                    Session["StudentTimes7"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 8)
                {
                    Session["StudentTimes8"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 9)
                {
                    Session["StudentTimes9"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 10)
                {
                    Session["StudentTimes10"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 11)
                {
                    Session["StudentTimes11"] = CountTimesInMonthCheck.Count();
                }
                else if (i == 12)
                {
                    Session["StudentTimes12"] = CountTimesInMonthCheck.Count();
                }
            }
            string StudentName = Session["DetailsPersonName"].ToString();
            int total = Convert.ToInt32(Session["TotalStudent"].ToString());
            double average = total / 12;
            Session["ReportForStudentTimes"] = new List<ReportForStudentTimes>();
            List<ReportForStudentTimes> rpfstt = Session["ReportForStudentTimes"] as List<ReportForStudentTimes>;
            ReportForStudentTimes newobj = new ReportForStudentTimes()
            {
                PeopleID = idstudent,
                Name = StudentName,
                TotalTime = total,
                Year = year,
                StudentTimesJan = Convert.ToInt32(Session["StudentTimes1"].ToString()),
                StudentTimesFeb = Convert.ToInt32(Session["StudentTimes2"].ToString()),
                StudentTimesMar = Convert.ToInt32(Session["StudentTimes3"].ToString()),
                StudentTimesApr = Convert.ToInt32(Session["StudentTimes4"].ToString()),
                StudentTimesMay = Convert.ToInt32(Session["StudentTimes5"].ToString()),
                StudentTimesJune = Convert.ToInt32(Session["StudentTimes6"].ToString()),
                StudentTimesJuly = Convert.ToInt32(Session["StudentTimes7"].ToString()),
                StudentTimesAug = Convert.ToInt32(Session["StudentTimes8"].ToString()),
                StudentTimesSep = Convert.ToInt32(Session["StudentTimes9"].ToString()),
                StudentTimesOct = Convert.ToInt32(Session["StudentTimes10"].ToString()),
                StudentTimesNov = Convert.ToInt32(Session["StudentTimes11"].ToString()),
                StudentTimesDec = Convert.ToInt32(Session["StudentTimes12"].ToString()),
                StudentAverageTimes = Math.Round(average),
            };
            rpfstt.Add(newobj);
            return View(rpfstt);
        }
        // GET: StudentTimes/Details/5
        public ActionResult ReportStudentTimeSkill(int year, string idstudent)
        {

            Session["ReportStudentTimeSkillYear"] = null;
            Session["ReportStudentTimeSkillIDStudent"] = null;
            Session["ReportStudentTimeSkillYear"] = year;
            Session["ReportStudentTimeSkillIDStudent"] = idstudent;
            Session["ReportForStudentTimesSkill"] = new List<ReportForStudentTimesSkill>();
            List<ReportForStudentTimesSkill> rpfsttk = Session["ReportForStudentTimesSkill"] as List<ReportForStudentTimesSkill>;
            var getallStudied = db.QuantityAndAttendances.Where(c => c.PeopleID == idstudent && c.UsingRoom.Date.Value.Year == year && c.UsingRoom.Date <= DateTime.Now);
            var listskills = db.Skills;
            foreach (var skill in listskills)
            {
                int counteachskill = 0;
                foreach (var studied in getallStudied.Where(c => c.UsingRoom.Class.Lesson.Topic.Skill.SkilID == skill.SkilID))
                {
                    counteachskill++;
                }
                float percent = (float)counteachskill / (float)getallStudied.Count();
                ReportForStudentTimesSkill newobj = new ReportForStudentTimesSkill()
                {
                    SKillName = skill.Name,
                    NumberOfStudiedSkill = counteachskill,
                    Percent = String.Format("{0:P2}", percent),
                };
                rpfsttk.Add(newobj);
            }
            return PartialView("../StudentTimes/ReportStudentTimeSkill", rpfsttk);
        }
        public JsonResult ReportStudentTimeSkillJson()
        {
            int year = Convert.ToInt32(Session["ReportStudentTimeSkillYear"].ToString());
            //int year = Convert.ToInt32(Session["ParaForReportStudentTime1Year"].ToString());

            string idstudent = Session["ReportStudentTimeSkillIDStudent"].ToString();
            Session["ReportForStudentTimesSkill"] = new List<ReportForStudentTimesSkill>();
            List<ReportForStudentTimesSkill> rpfsttkjs = Session["ReportForStudentTimesSkill"] as List<ReportForStudentTimesSkill>;
            var getallStudied = db.QuantityAndAttendances.Where(c => c.PeopleID == idstudent && c.UsingRoom.Date.Value.Year == year && c.UsingRoom.Date <= DateTime.Now);
            var listskills = db.Skills;
            foreach (var skill in listskills)
            {
                int counteachskill = 0;
                foreach (var studied in getallStudied.Where(c => c.UsingRoom.Class.Lesson.Topic.Skill.SkilID == skill.SkilID))
                {
                    counteachskill++;
                }
                float percent = (float)counteachskill / (float)getallStudied.Count();
                ReportForStudentTimesSkill newobj = new ReportForStudentTimesSkill()
                {
                    SKillName = skill.Name,
                    NumberOfStudiedSkill = counteachskill,
                    Percent = String.Format("{0:P2}", percent),
                };
                rpfsttkjs.Add(newobj);
            }
            return Json(new { rpfsttkjs }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTime studentTime = db.StudentTimes.Find(id);
            if (studentTime == null)
            {
                return HttpNotFound();
            }
            return View(studentTime);
        }

        // GET: StudentTimes/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.People, "PeopleID", "Name");
            return View();
        }

        // POST: StudentTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,Studied,Future")] StudentTime studentTime)
        {
            if (ModelState.IsValid)
            {
                db.StudentTimes.Add(studentTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.People, "PeopleID", "Name", studentTime.StudentID);
            return View(studentTime);
        }

        // GET: StudentTimes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTime studentTime = db.StudentTimes.Find(id);
            if (studentTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.People, "PeopleID", "Name", studentTime.StudentID);
            return View(studentTime);
        }

        // POST: StudentTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,Studied,Future")] StudentTime studentTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.People, "PeopleID", "Name", studentTime.StudentID);
            return View(studentTime);
        }

        // GET: StudentTimes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTime studentTime = db.StudentTimes.Find(id);
            if (studentTime == null)
            {
                return HttpNotFound();
            }
            return View(studentTime);
        }

        // POST: StudentTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StudentTime studentTime = db.StudentTimes.Find(id);
            db.StudentTimes.Remove(studentTime);
            db.SaveChanges();
            return RedirectToAction("Index");
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
