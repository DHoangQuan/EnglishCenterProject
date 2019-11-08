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
    public class QuantityAndAttendancesController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        public ActionResult HistoryForStudent(string idpeople)
        {
                var findhistory = db.QuantityAndAttendances.Where(c => c.PeopleID == idpeople);
                return View(findhistory);
        }

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

        public RedirectToRouteResult CancelUSR(int usrid, string peopleid, DateTime date)
        {
            var findtodel = db.QuantityAndAttendances.Where(c=>c.PeopleID == peopleid && c.UsingRoomID ==usrid).FirstOrDefault();
            //QuantityAndAttendance quantityAndAttendance = db.QuantityAndAttendances.Find(id);
            db.QuantityAndAttendances.Remove(findtodel);
            db.SaveChanges();
            return RedirectToAction("CountClassAndSlot", "UsingRooms", date);
        }

        public RedirectToRouteResult RegisterClass(QuantityAndAttendance qaa, int usrid, string peopleid, DateTime date)
        {
            UsingRoom checkquantityofstudent = db.UsingRooms.Find(usrid);
            var checkregister = db.QuantityAndAttendances.Where(c=>c.UsingRoom.SlotID == checkquantityofstudent.SlotID && c.UsingRoom.Date == date && c.PeopleID == peopleid);
            if(checkregister.Count() == 0)
            {
                if (checkquantityofstudent.QuatityRegister < checkquantityofstudent.Class.QuantityMAX || checkquantityofstudent.QuatityRegister == null)
                {
                    checkquantityofstudent.QuatityRegister++;
                    db.Entry(checkquantityofstudent).State = EntityState.Modified;
                    db.SaveChanges();
                    db = new ModelContext1();
                    // add student in class
                    qaa.QnAID = RandomString(25, true);
                    qaa.UsingRoomID = checkquantityofstudent.UsingRoomID;
                    qaa.PeopleID = peopleid;
                    qaa.Attendent = "Not Yet";
                    db.QuantityAndAttendances.Add(qaa);
                    db.SaveChanges();
                    return RedirectToAction("CountClassAndSlot", "UsingRooms", date);
                }
                else
                {
                    TempData["NoClass"] = "<script>alert('This class is full of student, please try another class');</script>";
                }
            }
            else
            {
                TempData["notification"] = "<script>alert('You registered other classes in this slot before. Please register for another class.');</script>";
            }
            

            return RedirectToAction("CountClassAndSlot", "UsingRooms", date);
            //return RedirectToAction("Index");
        }

        // GET: QuantityAndAttendances
        public ActionResult Index()
        {
            var quantityAndAttendances = db.QuantityAndAttendances.Include(q => q.Person).Include(q => q.UsingRoom);
            return View(quantityAndAttendances.ToList());
        }

        // GET: QuantityAndAttendances/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuantityAndAttendance quantityAndAttendance = db.QuantityAndAttendances.Find(id);
            if (quantityAndAttendance == null)
            {
                return HttpNotFound();
            }
            return View(quantityAndAttendance);
        }

        // GET: QuantityAndAttendances/Create
        public ActionResult Create()
        {
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name");
            ViewBag.UsingRoomID = new SelectList(db.UsingRooms, "UsingRoomID", "RoomID");
            return View();
        }

        // POST: QuantityAndAttendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QnAID,UsingRoomID,PeopleID,Attendent")] QuantityAndAttendance quantityAndAttendance)
        {
            if (ModelState.IsValid)
            {
                db.QuantityAndAttendances.Add(quantityAndAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", quantityAndAttendance.PeopleID);
            ViewBag.UsingRoomID = new SelectList(db.UsingRooms, "UsingRoomID", "RoomID", quantityAndAttendance.UsingRoomID);
            return View(quantityAndAttendance);
        }

        // GET: QuantityAndAttendances/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuantityAndAttendance quantityAndAttendance = db.QuantityAndAttendances.Find(id);
            if (quantityAndAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", quantityAndAttendance.PeopleID);
            ViewBag.UsingRoomID = new SelectList(db.UsingRooms, "UsingRoomID", "RoomID", quantityAndAttendance.UsingRoomID);
            return View(quantityAndAttendance);
        }

        // POST: QuantityAndAttendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QnAID,UsingRoomID,PeopleID,Attendent")] QuantityAndAttendance quantityAndAttendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quantityAndAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", quantityAndAttendance.PeopleID);
            ViewBag.UsingRoomID = new SelectList(db.UsingRooms, "UsingRoomID", "RoomID", quantityAndAttendance.UsingRoomID);
            return View(quantityAndAttendance);
        }

        // GET: QuantityAndAttendances/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuantityAndAttendance quantityAndAttendance = db.QuantityAndAttendances.Find(id);
            if (quantityAndAttendance == null)
            {
                return HttpNotFound();
            }
            return View(quantityAndAttendance);
        }

        // POST: QuantityAndAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuantityAndAttendance quantityAndAttendance = db.QuantityAndAttendances.Find(id);
            db.QuantityAndAttendances.Remove(quantityAndAttendance);
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
