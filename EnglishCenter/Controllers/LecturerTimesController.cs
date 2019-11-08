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
    public class LecturerTimesController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        

        public ActionResult AddNewTimeAndNewMonth(string id, DateTime date)
        {
            //var getyear = DateTime.Now.Year.ToString();
            //var getmonth = DateTime.Now.Month.ToString();
            var getyear = date.Year.ToString();
            var getmonth = date.Month.ToString();
            var checkyear = db.LecturerTimes.Where(t => t.LecturerID == id && t.Year == getyear);
            var checkmonth = db.LecturerTimes.Where(t => t.LecturerID == id && t.Month == getmonth);
            //Session["check"] = checkmonth.Count();
            if (checkyear.Count() == 0)
            {
                LecturerTime ltt = new LecturerTime();
                ltt.ID = RandomString(15, true);
                ltt.LecturerID = id;
                ltt.Year = getyear;
                ltt.Month = getmonth;
                ltt.TeachingTime = 1;
                db.LecturerTimes.Add(ltt);
                db.SaveChanges();
                db = new ModelContext1();
            }
            else if (checkmonth.Count() == 0)
            {
                LecturerTime ltt = new LecturerTime();
                ltt.ID = RandomString(15, true);
                ltt.LecturerID = id;
                ltt.Year = getyear;
                ltt.Month = getmonth;
                ltt.TeachingTime = 1;
                db.LecturerTimes.Add(ltt);
                db.SaveChanges();
                db = new ModelContext1();
                //return RedirectToAction("Index","LecturerTimes");
            }
            else
            {
                var lecturertimeteching = db.LecturerTimes.Where(c => c.LecturerID == id && c.Year == getyear && c.Month == getmonth);
                var resultid = lecturertimeteching.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                LecturerTime lecturerTime = db.LecturerTimes.Find(resultid.ID);
                lecturerTime.TeachingTime++;
                db.Entry(lecturerTime).State = EntityState.Modified;
                db.SaveChanges();
                db = new ModelContext1();
            }
            return View();
            //return RedirectToAction("Index", "LecturerTimes");
        }

        // GET: LecturerTimes
        public ActionResult Index()
        {
            var lecturerTimes = db.LecturerTimes.Include(l => l.Person);
            return View(lecturerTimes.ToList());
        }

        // GET: LecturerTimes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LecturerTime lecturerTime = db.LecturerTimes.Find(id);
            if (lecturerTime == null)
            {
                return HttpNotFound();
            }
            return View(lecturerTime);
        }

        // GET: LecturerTimes/Create
        public ActionResult Create()
        {
            ViewBag.LecturerID = new SelectList(db.People, "PeopleID", "Name");
            return View();
        }

        // POST: LecturerTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LecturerID,Year,Month,TeachingTime")] LecturerTime lecturerTime)
        {
            if (ModelState.IsValid)
            {
                lecturerTime.Month = DateTime.Now.Month.ToString();
                lecturerTime.ID = RandomString(15, true);
                db.LecturerTimes.Add(lecturerTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LecturerID = new SelectList(db.People, "PeopleID", "Name", lecturerTime.LecturerID);
            return View(lecturerTime);
        }

        // GET: LecturerTimes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LecturerTime lecturerTime = db.LecturerTimes.Find(id);
            if (lecturerTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.LecturerID = new SelectList(db.People, "PeopleID", "Name", lecturerTime.LecturerID);
            return View(lecturerTime);
        }

        // POST: LecturerTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LecturerID,Year,Month,TeachingTime")] LecturerTime lecturerTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecturerTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LecturerID = new SelectList(db.People, "PeopleID", "Name", lecturerTime.LecturerID);
            return View(lecturerTime);
        }

        // GET: LecturerTimes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LecturerTime lecturerTime = db.LecturerTimes.Find(id);
            if (lecturerTime == null)
            {
                return HttpNotFound();
            }
            return View(lecturerTime);
        }

        // POST: LecturerTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LecturerTime lecturerTime = db.LecturerTimes.Find(id);
            db.LecturerTimes.Remove(lecturerTime);
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
