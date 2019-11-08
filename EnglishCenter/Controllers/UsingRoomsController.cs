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
using System.Collections;
using System.Globalization;
using System.Net.Mail;

namespace EnglishCenter.Controllers
{
    public class UsingRoomsController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        // GET: UsingRooms
        public ActionResult Index()
        {
            var usingRooms = db.UsingRooms.Include(u => u.Class).Include(u => u.Room).Include(u => u.Slot);
            return View(usingRooms.ToList());
        }

        public ActionResult HistoryForLecturer(string idpeople)
        {
            var findhistory = db.UsingRooms.Where(c => c.Class.PeopleID == idpeople);
            return View(findhistory);
        }
        public ActionResult PartialIndex(string slot, string date)
        {
            DateTime datee = Convert.ToDateTime(date);
            var classfordayandslot = db.UsingRooms.Where(w => w.Date == datee && w.SlotID == slot);
            if(classfordayandslot.Count() != 0)
            {
                var autoname = classfordayandslot.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                ViewBag.tableslot = autoname.Slot.SlotName;
                ViewBag.tabledate = date;
            }
            
            return View(classfordayandslot);
        }

        public ActionResult IndexForSevenDay()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var listUS7days = db.UsingRooms.Where(c => c.Date >= date && c.Date <= date6);
            return PartialView("../UsingRooms/IndexForSevenDay", listUS7days);
        }
        public ActionResult IndexJustCreate()
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            DateTime date6 = date.AddDays(+6).Date;
            var listUSJustCreate = db.UsingRooms.Where(c => c.Status == "Just Create" && c.Date >= date && c.Date <= date6);
            return PartialView("../UsingRooms/IndexJustCreate", listUSJustCreate);
        }


        // GET: UsingRooms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsingRoom usingRoom = db.UsingRooms.Find(id);
            if (usingRoom == null)
            {
                return HttpNotFound();
            }
            return View(usingRoom);
        }

        // GET: UsingRooms/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName");
            ViewBag.RoomID = new SelectList(db.Rooms, "RommID", "RoomNo");
            ViewBag.SlotID = new SelectList(db.Slots, "SlotID", "SlotName");
            return View();
        }
        // for student
        public ActionResult IndexGet(string slot, DateTime date)
        {
            string idpeople = Session["PersonID"].ToString();
            var classinslotandday = db.UsingRooms.Where(c => c.Date == date && c.SlotID == slot);
            var check = db.QuantityAndAttendances.Where(c => c.PeopleID == idpeople);
            List<CheckButton> Usidlist = new List<CheckButton>();
            foreach (var student in check)
            {
                foreach (var item in classinslotandday.Where(d => d.UsingRoomID == student.UsingRoomID))
                {
                    string y = item.Slot.TimeStart;
                    TimeSpan startTimeClass = TimeSpan.Parse(y);
                    DateTime startDateTimeClass = (item.Date ?? DateTime.Now) + startTimeClass;
                    Usidlist.Add(new CheckButton()
                    {

                        UsingRoomID = item.UsingRoomID,
                        DateTime = startDateTimeClass,
                    });
                }
            }
            var ts = classinslotandday.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            ViewBag.timestartpara = ts.Slot.TimeStart.ToString();
            ViewBag.datepara = date.ToShortDateString();
            ViewBag.listcheck = Usidlist;
            return PartialView("../UsingRooms/IndexGet", classinslotandday);
        }

        public ActionResult EditForIndexGet(int idforedit)
        {
            ViewBag.idedit = idforedit;
            UsingRoom usingRoom = db.UsingRooms.Find(idforedit);
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "LessonID", usingRoom.ClassID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RommID", "Floor", usingRoom.RoomID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotID", "SlotName", usingRoom.SlotID);
            return PartialView("../UsingRooms/EditForIndexGet", usingRoom);
        }
        [HttpPost]
        public ActionResult EditForIndexGet(int idd, string idroom, string idslot, DateTime iddate, string iddayofweek, string idclassid, string idusedfor, string idstatus, string idnote)
        //public ActionResult EditForIndexGet(UsingRoom usingRoom)

        {

            UsingRoom usingRoom = new UsingRoom();
            usingRoom.UsingRoomID = idd;
            usingRoom.RoomID = idroom;
            usingRoom.SlotID = idslot;
            usingRoom.Date = iddate;
            usingRoom.DayOfWeek = iddayofweek;
            usingRoom.ClassID = idclassid;
            usingRoom.UseFor = idusedfor;
            usingRoom.Status = idstatus;
            usingRoom.Notes = idnote;
            db.Entry(usingRoom).State = EntityState.Modified;
            db.SaveChanges();

            //if (ModelState.IsValid)
            //{
            //    db.Entry(usingRoom).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            //ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "LessonID", usingRoom.ClassID);
            //ViewBag.RoomID = new SelectList(db.Rooms, "RommID", "Floor", usingRoom.RoomID);
            //ViewBag.SlotID = new SelectList(db.Slots, "SlotID", "SlotName", usingRoom.SlotID);

            //return Json(new { data = "Success" }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("CustomeCreate", "UsingRooms", new { datestr1 = @Session["firstday"], check = "a" });
        }
        public ActionResult IndexLesson(string lessonid)
        {
            //ViewBag.ABC = slot;
            var lesson = db.Lessons.Where(c => c.LessonID == lessonid);

            return PartialView("../UsingRooms/IndexLesson", lesson);
        }
        public ActionResult ScheduleForTeacher(DateTime? date12)
        {
            DateTime date1 = date12 ?? DateTime.Now;
            string teacherid = Session["PersonID"].ToString();
            var usingroombyday = db.UsingRooms.Where(d => d.Date == date1 && d.Class.PeopleID == teacherid);
            if (Session["SWeekRoom"] == null)
            {
                Session["SWeekRoom"] = new List<WeekContainClass>();
            }
            List<WeekContainClass> wcl = Session["SWeekRoom"] as List<WeekContainClass>;
            foreach (var usr in usingroombyday)
            {
                WeekContainClass newWeek = new WeekContainClass()
                {
                    UsingRoomID = usr.UsingRoomID,
                    RoomID = usr.RoomID,
                    SlotID = usr.SlotID,
                    Date = usr.Date,
                    DayOfWeek = usr.DayOfWeek,
                    ClassID = usr.ClassID,
                    LessonID = usr.Class.LessonID,
                    UseFor = usr.UseFor,
                    Notes = usr.Notes,
                    TimeStart = usr.Slot.TimeStart,
                    TimeEnd = usr.Slot.TimeEnd,
                    ClassName = usr.Class.ClassName + " - " + usr.Class.Person.Name,
                };
                wcl.Add(newWeek);
            }
            return View(wcl);
        }

        //public ActionResult Calculate2(DateTime? date34)
        public ActionResult Calculate2(DateTime date)
        {
            if (Session["SWeekRoom"] != null)
            {
                List<WeekContainClass> wcl = Session["SWeekRoom"] as List<WeekContainClass>;
                wcl.Clear();
            }
            Session["Monday2"] = null;
            Session["Tuesday2"] = null;
            Session["Wednesday2"] = null;
            Session["Thursday2"] = null;
            Session["Friday2"] = null;
            Session["Saturday2"] = null;
            Session["Sunday2"] = null;
            if (date.DayOfWeek.ToString() == "Monday")
            {
                Session["Monday2"] = date.ToShortDateString();
                Session["Tuesday2"] = date.AddDays(+1).ToShortDateString();
                Session["Wednesday2"] = date.AddDays(+2).ToShortDateString();
                Session["Thursday2"] = date.AddDays(+3).ToShortDateString();
                Session["Friday2"] = date.AddDays(+4).ToShortDateString();
                Session["Saturday2"] = date.AddDays(+5).ToShortDateString();
                Session["Sunday2"] = date.AddDays(+6).ToShortDateString();
                ScheduleForTeacher(date);
                ScheduleForTeacher(date.AddDays(+1));
                ScheduleForTeacher(date.AddDays(+2));
                ScheduleForTeacher(date.AddDays(+3));
                ScheduleForTeacher(date.AddDays(+4));
                ScheduleForTeacher(date.AddDays(+5));
                ScheduleForTeacher(date.AddDays(+6));
            }
            else if (date.DayOfWeek.ToString() == "Tuesday")
            {
                Session["Monday2"] = date.AddDays(-1).ToShortDateString();
                Session["Tuesday2"] = date.ToShortDateString();
                Session["Wednesday2"] = date.AddDays(+1).ToShortDateString();
                Session["Thursday2"] = date.AddDays(+2).ToShortDateString();
                Session["Friday2"] = date.AddDays(+3).ToShortDateString();
                Session["Saturday2"] = date.AddDays(+4).ToShortDateString();
                Session["Sunday2"] = date.AddDays(+5).ToShortDateString();
                ScheduleForTeacher(date.AddDays(-1));
                ScheduleForTeacher(date);
                ScheduleForTeacher(date.AddDays(+1));
                ScheduleForTeacher(date.AddDays(+2));
                ScheduleForTeacher(date.AddDays(+3));
                ScheduleForTeacher(date.AddDays(+4));
                ScheduleForTeacher(date.AddDays(+5));
            }
            else if (date.DayOfWeek.ToString() == "Wednesday")
            {
                Session["Monday2"] = date.AddDays(-2).ToShortDateString();
                Session["Tuesday2"] = date.AddDays(-1).ToShortDateString();
                Session["Wednesday2"] = date.ToShortDateString();
                Session["Thursday2"] = date.AddDays(+1).ToShortDateString();
                Session["Friday2"] = date.AddDays(+2).ToShortDateString();
                Session["Saturday2"] = date.AddDays(+3).ToShortDateString();
                Session["Sunday2"] = date.AddDays(+4).ToShortDateString();
                ScheduleForTeacher(date.AddDays(-2));
                ScheduleForTeacher(date.AddDays(-1));
                ScheduleForTeacher(date);
                ScheduleForTeacher(date.AddDays(+1));
                ScheduleForTeacher(date.AddDays(+2));
                ScheduleForTeacher(date.AddDays(+3));
                ScheduleForTeacher(date.AddDays(+4));
            }
            else if (date.DayOfWeek.ToString() == "Thursday")
            {
                Session["Monday2"] = date.AddDays(-3).ToShortDateString();
                Session["Tuesday2"] = date.AddDays(-2).ToShortDateString();
                Session["Wednesday2"] = date.AddDays(-1).ToShortDateString();
                Session["Thursday2"] = date.ToShortDateString();
                Session["Friday2"] = date.AddDays(+1).ToShortDateString();
                Session["Saturday2"] = date.AddDays(+2).ToShortDateString();
                Session["Sunday2"] = date.AddDays(+3).ToShortDateString();
                ScheduleForTeacher(date.AddDays(-3));
                ScheduleForTeacher(date.AddDays(-2));
                ScheduleForTeacher(date.AddDays(-1));
                ScheduleForTeacher(date);
                ScheduleForTeacher(date.AddDays(+1));
                ScheduleForTeacher(date.AddDays(+2));
                ScheduleForTeacher(date.AddDays(+3));
            }
            else if (date.DayOfWeek.ToString() == "Friday")
            {
                Session["Monday2"] = date.AddDays(-4).ToShortDateString();
                Session["Tuesday2"] = date.AddDays(-3).ToShortDateString();
                Session["Wednesday2"] = date.AddDays(-2).ToShortDateString();
                Session["Thursday2"] = date.AddDays(-1).ToShortDateString();
                Session["Friday2"] = date.ToShortDateString();
                Session["Saturday2"] = date.AddDays(+1).ToShortDateString();
                Session["Sunday2"] = date.AddDays(+2).ToShortDateString();
                ScheduleForTeacher(date.AddDays(-4));
                ScheduleForTeacher(date.AddDays(-3));
                ScheduleForTeacher(date.AddDays(-2));
                ScheduleForTeacher(date.AddDays(-1));
                ScheduleForTeacher(date);
                ScheduleForTeacher(date.AddDays(+1));
                ScheduleForTeacher(date.AddDays(+2));

            }
            else if (date.DayOfWeek.ToString() == "Saturday")
            {
                Session["Monday2"] = date.AddDays(-5).ToShortDateString();
                Session["Tuesday2"] = date.AddDays(-4).ToShortDateString();
                Session["Wednesday2"] = date.AddDays(-3).ToShortDateString();
                Session["Thursday2"] = date.AddDays(-2).ToShortDateString();
                Session["Friday2"] = date.AddDays(-1).ToShortDateString();
                Session["Saturday2"] = date.ToShortDateString();
                Session["Sunday2"] = date.AddDays(+1).ToShortDateString();
                ScheduleForTeacher(date.AddDays(-5));
                ScheduleForTeacher(date.AddDays(-4));
                ScheduleForTeacher(date.AddDays(-3));
                ScheduleForTeacher(date.AddDays(-2));
                ScheduleForTeacher(date.AddDays(-1));
                ScheduleForTeacher(date);
                ScheduleForTeacher(date.AddDays(+1));

            }
            else if (date.DayOfWeek.ToString() == "Sunday")
            {
                Session["Monday2"] = date.AddDays(-6).ToShortDateString();
                Session["Tuesday2"] = date.AddDays(-5).ToShortDateString();
                Session["Wednesday2"] = date.AddDays(-4).ToShortDateString();
                Session["Thursday2"] = date.AddDays(-3).ToShortDateString();
                Session["Friday2"] = date.AddDays(-2).ToShortDateString();
                Session["Saturday2"] = date.AddDays(-1).ToShortDateString();
                Session["Sunday2"] = date.ToShortDateString();
                ScheduleForTeacher(date.AddDays(-6));
                ScheduleForTeacher(date.AddDays(-5));
                ScheduleForTeacher(date.AddDays(-4));
                ScheduleForTeacher(date.AddDays(-3));
                ScheduleForTeacher(date.AddDays(-2));
                ScheduleForTeacher(date.AddDays(-1));
                ScheduleForTeacher(date);

            }
            //return View();
            return RedirectToAction("ScheduleForTeacher", "UsingRooms");

        }

        public ActionResult CountClassAndSlot(DateTime? date1)
        {
            DateTime date = date1 ?? DateTime.Now;
            
            var listslot = db.Slots;
            var listskill = db.Skills;
            var classbydate = db.UsingRooms.Where(d => d.Date == date);
            if (Session["SWeekManyRoom"] == null)
            {
                Session["SWeekManyRoom"] = new List<WeekContainClassAndSlot>();
            }
            //List<WeekContainClassAndSlot> wcls = new List<WeekContainClassAndSlot>();
            List<WeekContainClassAndSlot> wcls = Session["SWeekManyRoom"] as List<WeekContainClassAndSlot>;
            foreach (var slot in listslot)
            {
                var CountClassInSlot = 0;
                var readingskill = 0;
                var listeningskill = 0;
                var writingskill = 0;
                var speakingskill = 0;
                foreach (var item in classbydate.Where(c => c.SlotID == slot.SlotID))
                {
                    CountClassInSlot++;
                }
                foreach (var skill in listskill)
                {
                    foreach (var item1 in classbydate.Where(c => c.SlotID == slot.SlotID && c.Class.Lesson.Topic.Skill.SkilID == skill.SkilID))
                    {
                        if (skill.SkilID == "SKILL01")
                        {
                            listeningskill++;
                        }
                        if (skill.SkilID == "SKILL02")
                        {
                            readingskill++;
                        }
                        if (skill.SkilID == "SKILL03")
                        {
                            writingskill++;
                        }
                        if (skill.SkilID == "SKILL04")
                        {
                            speakingskill++;
                        }
                    }
                }

                WeekContainClassAndSlot ManyRoom = new WeekContainClassAndSlot()
                {
                    NumberofClassUsingRoom = CountClassInSlot,
                    SlotID = slot.SlotID,
                    Date = date,
                    DayOfWeek = date.DayOfWeek.ToString(),
                    TimeStart = slot.TimeStart,
                    TimeEnd = slot.TimeEnd,
                    numberofreadingskill = readingskill,
                    numberofspeakingskill = speakingskill,
                    numberofwritingskill = writingskill,
                    numeberoflisteningskill = listeningskill,
                };
                wcls.Add(ManyRoom);
            }
            return View(wcls);
        }
        public ActionResult Calculate1(DateTime date)
        {
            if (Session["SWeekManyRoom"] != null)
            {
                List<WeekContainClassAndSlot> wcls = Session["SWeekManyRoom"] as List<WeekContainClassAndSlot>;
                wcls.Clear();
            }
            Session["timepara"] = null;
            Session["Monday"] = null;
            Session["Tuesday"] = null;
            Session["Wednesday"] = null;
            Session["Thursday"] = null;
            Session["Friday"] = null;
            Session["Saturday"] = null;
            Session["Sunday"] = null;
            if (date.DayOfWeek.ToString() == "Monday")
            {
                CountClassAndSlot(date);
                Session["timepara"] = date.ToShortDateString();
                Session["Monday"] = date.ToShortDateString();
                CountClassAndSlot(date.AddDays(+1));               
                Session["Tuesday"] = (date.AddDays(+1)).ToShortDateString();
                CountClassAndSlot(date.AddDays(+2));
                Session["Wednesday"] = date.AddDays(+2).ToShortDateString();
                CountClassAndSlot(date.AddDays(+3));
                Session["Thursday"] = date.AddDays(+3).ToShortDateString();
                CountClassAndSlot(date.AddDays(+4));
                Session["Friday"] = date.AddDays(+4).ToShortDateString();
                CountClassAndSlot(date.AddDays(+5));
                Session["Saturday"] = date.AddDays(+5).ToShortDateString();
                CountClassAndSlot(date.AddDays(+6));
                Session["Sunday"] = date.AddDays(+6).ToShortDateString();
            }
            else if (date.DayOfWeek.ToString() == "Tuesday")
            {     
                CountClassAndSlot(date.AddDays(-1));
                Session["timepara"] = date.AddDays(-1).ToShortDateString();
                Session["Monday"] = date.AddDays(-1).ToShortDateString();
                CountClassAndSlot(date);
                Session["Tuesday"] = date.ToShortDateString();
                CountClassAndSlot(date.AddDays(+1));
                Session["Wednesday"] = (date.AddDays(+1)).ToShortDateString();
                CountClassAndSlot(date.AddDays(+2));
                Session["Thursday"] = date.AddDays(+2).ToShortDateString();
                CountClassAndSlot(date.AddDays(+3));
                Session["Friday"] = date.AddDays(+3).ToShortDateString();
                CountClassAndSlot(date.AddDays(+4));
                Session["Saturday"] = date.AddDays(+4).ToShortDateString();
                CountClassAndSlot(date.AddDays(+5));
                Session["Sunday"] = date.AddDays(+5).ToShortDateString();
            }
            else if (date.DayOfWeek.ToString() == "Wednesday")
            {
                CountClassAndSlot(date.AddDays(-2));
                Session["timepara"] = date.AddDays(-2).ToShortDateString();
                Session["Monday"] = date.AddDays(-2).ToShortDateString();
                CountClassAndSlot(date.AddDays(-1));
                Session["Tuesday"] = date.AddDays(-1).ToShortDateString();
                CountClassAndSlot(date);
                Session["Wednesday"] = date.ToShortDateString();
                CountClassAndSlot(date.AddDays(+1));
                Session["Thursday"] = date.AddDays(+1).ToShortDateString();
                CountClassAndSlot(date.AddDays(+2));
                Session["Friday"] = date.AddDays(+2).ToShortDateString();
                CountClassAndSlot(date.AddDays(+3));
                Session["Saturday"] = date.AddDays(+3).ToShortDateString();
                CountClassAndSlot(date.AddDays(+4));
                Session["Sunday"] = date.AddDays(+4).ToShortDateString();
            }
            else if (date.DayOfWeek.ToString() == "Thursday")
            {
                CountClassAndSlot(date.AddDays(-3));
                Session["timepara"] = date.AddDays(-3).ToShortDateString();
                Session["Monday"] = date.AddDays(-3).ToShortDateString();
                CountClassAndSlot(date.AddDays(-2));
                Session["Tuesday"] = date.AddDays(-2).ToShortDateString();
                CountClassAndSlot(date.AddDays(-1));
                Session["Wednesday"] = date.AddDays(-1).ToShortDateString();
                CountClassAndSlot(date);
                Session["Thursday"] = date.ToShortDateString();
                CountClassAndSlot(date.AddDays(+1));
                Session["Friday"] = date.AddDays(+1).ToShortDateString();
                CountClassAndSlot(date.AddDays(+2));
                Session["Saturday"] = date.AddDays(+2).ToShortDateString();
                CountClassAndSlot(date.AddDays(+3));
                Session["Sunday"] = date.AddDays(+3).ToShortDateString();
            }
            else if (date.DayOfWeek.ToString() == "Friday")
            {
                CountClassAndSlot(date.AddDays(-4));
                Session["timepara"] = date.AddDays(-4).ToShortDateString();
                Session["Monday"] = date.AddDays(-4).ToShortDateString();
                CountClassAndSlot(date.AddDays(-3));
                Session["Tuesday"] = date.AddDays(-3).ToShortDateString();
                CountClassAndSlot(date.AddDays(-2));
                Session["Wednesday"] = date.AddDays(-2).ToShortDateString();
                CountClassAndSlot(date.AddDays(-1));
                Session["Thursday"] = date.AddDays(-1).ToShortDateString();
                CountClassAndSlot(date);
                Session["Friday"] = date.ToShortDateString();
                CountClassAndSlot(date.AddDays(+1));
                Session["Saturday"] = date.AddDays(+1).ToShortDateString();
                CountClassAndSlot(date.AddDays(+2));
                Session["Sunday"] = date.AddDays(+2).ToShortDateString();
            }
            else if (date.DayOfWeek.ToString() == "Saturday")
            {
                CountClassAndSlot(date.AddDays(-5));
                Session["timepara"] = date.AddDays(-5).ToShortDateString();
                Session["Monday"] = date.AddDays(-5).ToShortDateString();
                CountClassAndSlot(date.AddDays(-4));
                Session["Tuesday"] = date.AddDays(-4).ToShortDateString();
                CountClassAndSlot(date.AddDays(-3));
                Session["Wednesday"] = date.AddDays(-3).ToShortDateString();
                CountClassAndSlot(date.AddDays(-2));
                Session["Thursday"] = date.AddDays(-2).ToShortDateString();
                CountClassAndSlot(date.AddDays(-1));
                Session["Friday"] = date.AddDays(-1).ToShortDateString();
                CountClassAndSlot(date);
                Session["Saturday"] = date.ToShortDateString();
                CountClassAndSlot(date.AddDays(+1));
                Session["Sunday"] = date.AddDays(+1).ToShortDateString();
            }
            else if (date.DayOfWeek.ToString() == "Sunday")
            {
                CountClassAndSlot(date.AddDays(-6));
                Session["timepara"] = date.AddDays(-6).ToShortDateString();
                Session["Monday"] = date.AddDays(-6).ToShortDateString();
                CountClassAndSlot(date.AddDays(-5));
                Session["Tuesday"] = date.AddDays(-5).ToShortDateString();
                CountClassAndSlot(date.AddDays(-4));
                Session["Wednesday"] = date.AddDays(-4).ToShortDateString();
                CountClassAndSlot(date.AddDays(-3));
                Session["Thursday"] = date.AddDays(-3).ToShortDateString();
                CountClassAndSlot(date.AddDays(-2));
                Session["Friday"] = date.AddDays(-2).ToShortDateString();
                CountClassAndSlot(date.AddDays(-1));
                Session["Saturday"] = date.AddDays(-1).ToShortDateString();
                CountClassAndSlot(date);
                Session["Sunday"] = date.ToShortDateString();
            }
            return RedirectToAction("CountClassAndSlot", "UsingRooms");
        }
        public ActionResult GetObjectWithDate(DateTime? date12)
        {

            DateTime date1 = date12 ?? DateTime.Now;
            string idpeople = Session["PersonID"].ToString();
            var findUSR = db.QuantityAndAttendances.Where(c=>c.PeopleID == idpeople && c.UsingRoom.Date == date12);
            //var usingroombyday = db.UsingRooms.Where(d => d.Date == date1);
            if (Session["SWeekRoom"] == null)
            {
                Session["SWeekRoom"] = new List<WeekContainClass>();
            }
            List<WeekContainClass> wcl = Session["SWeekRoom"] as List<WeekContainClass>;
            foreach (var usr in findUSR)
            {
                WeekContainClass newWeek = new WeekContainClass()
                {
                    UsingRoomID = usr.UsingRoomID ?? 1,
                    RoomID = usr.UsingRoom.RoomID,
                    SlotID = usr.UsingRoom.SlotID,
                    Date = usr.UsingRoom.Date,
                    DayOfWeek = usr.UsingRoom.DayOfWeek,
                    ClassID = usr.UsingRoom.ClassID,
                    LessonID = usr.UsingRoom.Class.LessonID,
                    UseFor = usr.UsingRoom.UseFor,
                    Notes = usr.UsingRoom.Notes,
                    TimeStart = usr.UsingRoom.Slot.TimeStart,
                    TimeEnd = usr.UsingRoom.Slot.TimeEnd,
                    ClassName = usr.UsingRoom.Class.ClassName + " - " + " Mr/Mrs " + usr.UsingRoom.Class.Person.Name,
                };
                wcl.Add(newWeek);
            }
            //foreach (var usr in usingroombyday)
            //{
            //    WeekContainClass newWeek = new WeekContainClass()
            //    {
            //        UsingRoomID = usr.UsingRoomID,
            //        RoomID = usr.RoomID,
            //        SlotID = usr.SlotID,
            //        Date = usr.Date,
            //        DayOfWeek = usr.DayOfWeek,
            //        ClassID = usr.ClassID,
            //        LessonID = usr.Class.LessonID,
            //        UseFor = usr.UseFor,
            //        Notes = usr.Notes,
            //        TimeStart = usr.Slot.TimeStart,
            //        TimeEnd = usr.Slot.TimeEnd,
            //        ClassName = usr.Class.ClassName + " - " +usr.Class.Person.Name,
            //    };
            //    wcl.Add(newWeek);
            //}
            return View(wcl);
            //return RedirectToAction("GetObjectWithDate", "UsingRooms");
        }

        //public ActionResult Calculate(DateTime? date34)
        public ActionResult Calculate(DateTime date)
        {
            if (Session["SWeekRoom"] != null)
            {
                List<WeekContainClass> wcl = Session["SWeekRoom"] as List<WeekContainClass>;
                wcl.Clear();
            }
            Session["Monday1"] = null;
            Session["Tuesday1"] = null;
            Session["Wednesday1"] = null;
            Session["Thursday1"] = null;
            Session["Friday1"] = null;
            Session["Saturday1"] = null;
            Session["Sunday1"] = null;
            if (date.DayOfWeek.ToString() == "Monday")
            {
                Session["Monday1"] = date.ToShortDateString();
                Session["Tuesday1"] = date.AddDays(+1).ToShortDateString();
                Session["Wednesday1"] = date.AddDays(+2).ToShortDateString();
                Session["Thursday1"] = date.AddDays(+3).ToShortDateString();
                Session["Friday1"] = date.AddDays(+4).ToShortDateString();
                Session["Saturday1"] = date.AddDays(+5).ToShortDateString();
                Session["Sunday1"] = date.AddDays(+6).ToShortDateString();
                GetObjectWithDate(date);
                GetObjectWithDate(date.AddDays(+1));
                GetObjectWithDate(date.AddDays(+2));
                GetObjectWithDate(date.AddDays(+3));
                GetObjectWithDate(date.AddDays(+4));
                GetObjectWithDate(date.AddDays(+5));
                GetObjectWithDate(date.AddDays(+6));
            }
            else if (date.DayOfWeek.ToString() == "Tuesday")
            {
                Session["Monday1"] = date.AddDays(-1).ToShortDateString();
                Session["Tuesday1"] = date.ToShortDateString();
                Session["Wednesday1"] = date.AddDays(+1).ToShortDateString();
                Session["Thursday1"] = date.AddDays(+2).ToShortDateString();
                Session["Friday1"] = date.AddDays(+3).ToShortDateString();
                Session["Saturday1"] = date.AddDays(+4).ToShortDateString();
                Session["Sunday1"] = date.AddDays(+5).ToShortDateString();
                GetObjectWithDate(date.AddDays(-1));
                GetObjectWithDate(date);
                GetObjectWithDate(date.AddDays(+1));
                GetObjectWithDate(date.AddDays(+2));
                GetObjectWithDate(date.AddDays(+3));
                GetObjectWithDate(date.AddDays(+4));
                GetObjectWithDate(date.AddDays(+5));
            }
            else if (date.DayOfWeek.ToString() == "Wednesday")
            {
                Session["Monday1"] = date.AddDays(-2).ToShortDateString();
                Session["Tuesday1"] = date.AddDays(-1).ToShortDateString();
                Session["Wednesday1"] = date.ToShortDateString();
                Session["Thursday1"] = date.AddDays(+1).ToShortDateString();
                Session["Friday1"] = date.AddDays(+2).ToShortDateString();
                Session["Saturday1"] = date.AddDays(+3).ToShortDateString();
                Session["Sunday1"] = date.AddDays(+4).ToShortDateString();
                GetObjectWithDate(date.AddDays(-2));
                GetObjectWithDate(date.AddDays(-1));
                GetObjectWithDate(date);
                GetObjectWithDate(date.AddDays(+1));
                GetObjectWithDate(date.AddDays(+2));
                GetObjectWithDate(date.AddDays(+3));
                GetObjectWithDate(date.AddDays(+4));
            }
            else if (date.DayOfWeek.ToString() == "Thursday")
            {
                Session["Monday1"] = date.AddDays(-3).ToShortDateString();
                Session["Tuesday1"] = date.AddDays(-2).ToShortDateString();
                Session["Wednesday1"] = date.AddDays(-1).ToShortDateString();
                Session["Thursday1"] = date.ToShortDateString();
                Session["Friday1"] = date.AddDays(+1).ToShortDateString();
                Session["Saturday1"] = date.AddDays(+2).ToShortDateString();
                Session["Sunday1"] = date.AddDays(+3).ToShortDateString();
                GetObjectWithDate(date.AddDays(-3));
                GetObjectWithDate(date.AddDays(-2));
                GetObjectWithDate(date.AddDays(-1));
                GetObjectWithDate(date);
                GetObjectWithDate(date.AddDays(+1));
                GetObjectWithDate(date.AddDays(+2));
                GetObjectWithDate(date.AddDays(+3));
            }
            else if (date.DayOfWeek.ToString() == "Friday")
            {
                Session["Monday1"] = date.AddDays(-4).ToShortDateString();
                Session["Tuesday1"] = date.AddDays(-3).ToShortDateString();
                Session["Wednesday1"] = date.AddDays(-2).ToShortDateString();
                Session["Thursday1"] = date.AddDays(-1).ToShortDateString();
                Session["Friday1"] = date.ToShortDateString();
                Session["Saturday1"] = date.AddDays(+1).ToShortDateString();
                Session["Sunday1"] = date.AddDays(+2).ToShortDateString();
                GetObjectWithDate(date.AddDays(-4));
                GetObjectWithDate(date.AddDays(-3));
                GetObjectWithDate(date.AddDays(-2));
                GetObjectWithDate(date.AddDays(-1));
                GetObjectWithDate(date);
                GetObjectWithDate(date.AddDays(+1));
                GetObjectWithDate(date.AddDays(+2));

            }
            else if (date.DayOfWeek.ToString() == "Saturday")
            {
                Session["Monday1"] = date.AddDays(-5).ToShortDateString();
                Session["Tuesday1"] = date.AddDays(-4).ToShortDateString();
                Session["Wednesday1"] = date.AddDays(-3).ToShortDateString();
                Session["Thursday1"] = date.AddDays(-2).ToShortDateString();
                Session["Friday1"] = date.AddDays(-1).ToShortDateString();
                Session["Saturday1"] = date.ToShortDateString();
                Session["Sunday1"] = date.AddDays(+1).ToShortDateString();
                GetObjectWithDate(date.AddDays(-5));
                GetObjectWithDate(date.AddDays(-4));
                GetObjectWithDate(date.AddDays(-3));
                GetObjectWithDate(date.AddDays(-2));
                GetObjectWithDate(date.AddDays(-1));
                GetObjectWithDate(date);
                GetObjectWithDate(date.AddDays(+1));

            }
            else if (date.DayOfWeek.ToString() == "Sunday")
            {
                Session["Monday1"] = date.AddDays(-6).ToShortDateString();
                Session["Tuesday1"] = date.AddDays(-5).ToShortDateString();
                Session["Wednesday1"] = date.AddDays(-4).ToShortDateString();
                Session["Thursday1"] = date.AddDays(-3).ToShortDateString();
                Session["Friday1"] = date.AddDays(-2).ToShortDateString();
                Session["Saturday1"] = date.AddDays(-1).ToShortDateString();
                Session["Sunday1"] = date.ToShortDateString();
                GetObjectWithDate(date.AddDays(-6));
                GetObjectWithDate(date.AddDays(-5));
                GetObjectWithDate(date.AddDays(-4));
                GetObjectWithDate(date.AddDays(-3));
                GetObjectWithDate(date.AddDays(-2));
                GetObjectWithDate(date.AddDays(-1));
                GetObjectWithDate(date);

            }
            //return View();
            return RedirectToAction("GetObjectWithDate", "UsingRooms");

        }
        // POST: UsingRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsingRoomID,RoomID,SlotID,Date,DayOfWeek,ClassID,UseFor,Notes")] UsingRoom usingRoom)
        {
            DateTime dt = usingRoom.Date ?? DateTime.Now;
            //DateTime abc = dt.AddDays(+2);
            if (ModelState.IsValid)
            {
                //usingRoom.UseFor = abc.ToString();
                usingRoom.DayOfWeek = dt.DayOfWeek.ToString();
                usingRoom.Status = "Just Create";
                db.UsingRooms.Add(usingRoom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", usingRoom.ClassID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RommID", "RoomNo", usingRoom.RoomID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotID", "SlotName", usingRoom.SlotID);
            return View(usingRoom);
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
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public ActionResult IndexTest()
        {

            return View();
        }
        public ActionResult IndexPopupFor7days(DateTime date, string slot)
        {
            var classfordayandslot = db.UsingRooms.Where(w => w.Date == date && w.SlotID == slot);
            return PartialView("../UsingRooms/IndexPopupFor7days", classfordayandslot);
        }
        public ActionResult AutoRoom(DateTime datestr, string slot)
        {

            if (Session["roomnotused"] != null)
            {
                List<RoomsHaveNotBeUsed> wcls = Session["roomnotused"] as List<RoomsHaveNotBeUsed>;
                wcls.Clear();
            }

            var usedroom = db.UsingRooms.Where(c => c.Date == datestr && c.SlotID == slot);

            if (usedroom == null)
            {
                var listroom = db.Rooms.ToList();
                var autoroom = listroom.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                ViewBag.AutoRoom = autoroom.RommID.ToString();
                //Session["count3"] = autoroom.RommID;
            }
            else
            {
                var listroom = db.Rooms;
                var dem = 0;
                Session["roomnotused"] = new List<RoomsHaveNotBeUsed>();
                List<RoomsHaveNotBeUsed> rhnbu = Session["roomnotused"] as List<RoomsHaveNotBeUsed>;
                foreach (var room in listroom)
                {
                    var x = room.RommID;
                    var result = usedroom.SingleOrDefault(c => c.RoomID == x);
                    var getif = db.Rooms.SingleOrDefault(c => c.RommID == x);
                    if (result == null)
                    {
                        dem = dem + 1;
                        RoomsHaveNotBeUsed ifr = new RoomsHaveNotBeUsed()
                        {
                            RommID = getif.RommID,
                            Floor = getif.Floor,
                            RoomNo = getif.RoomNo,
                        };
                        rhnbu.Add(ifr);
                    }
                }
                // chọn ngẫu nhiên trong list
                var autoroom = rhnbu.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                ViewBag.AutoRoom = autoroom.RommID.ToString();

            }
            return View();
        }

        public ActionResult AutoLesson(DateTime date, string slot)
        {
            if (Session["checklessons"] != null)
            {
                List<LessonHasNoBeTaught> lhnbt = Session["checklessons"] as List<LessonHasNoBeTaught>;
                lhnbt.Clear();
            }
            //DateTime trythis = datestr.AddDays(+i).Date;
            var usedroom = db.UsingRooms.Where(c => c.Date == date && c.SlotID == slot);
            if (usedroom == null)
            {
                var listlessons = db.Lessons.ToList();
                var autolesson = listlessons.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                var lessonid = autolesson.LessonID.ToString();
                var lessonname = autolesson.LessonName.ToString();
                ViewBag.AutoLesson = lessonid;
                ViewBag.AutoLessonName = lessonname;
                Lesson lesson = db.Lessons.Find(lessonid);
                lesson.RepeatTimes++;
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                db = new ModelContext1();
            }
            else
            {
                var listlessons = db.Lessons.ToList();
                Session["checklessons"] = new List<LessonHasNoBeTaught>();
                List<LessonHasNoBeTaught> checklessons = Session["checklessons"] as List<LessonHasNoBeTaught>;
                foreach (var lessons in listlessons)
                {
                    var x = lessons.LessonID;
                    var result = usedroom.SingleOrDefault(c => c.Class.LessonID == x);
                    var getif = db.Lessons.SingleOrDefault(c => c.LessonID == x);
                    if (result == null)
                    {
                        LessonHasNoBeTaught ifls = new LessonHasNoBeTaught()
                        {
                            LessonID = getif.LessonID,
                            LessonName = getif.LessonName,
                            TopicID = getif.TopicID,
                            Topicname = getif.Topic.TopicName,
                            RepeatTimes = getif.RepeatTimes,
                        };
                        checklessons.Add(ifls);
                    }
                }
                var autolessons = checklessons.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                var lessonid = autolessons.LessonID.ToString();
                var lessonname = autolessons.LessonName.ToString();
                ViewBag.AutoLesson = lessonid;
                ViewBag.AutoLessonName = lessonname;
                Lesson lesson = db.Lessons.Find(lessonid);
                lesson.RepeatTimes++;
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                db = new ModelContext1();
            }
            return View();
        }
        
        public ActionResult AutoLecturer(DateTime date, string slot)
        {
            if (Session["checklecturer"] != null)
            {
                List<TeacherDoNotTeach> ckl = Session["checklecturer"] as List<TeacherDoNotTeach>;
                ckl.Clear();
            }
            var usedroom = db.UsingRooms.Where(c => c.Date == date && c.SlotID == slot);
            if (usedroom == null)
            {
                var listlecturer = db.People.Where(c => c.Role.Role1 == "Lecturer");
                var autolecturer = listlecturer.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                var lecturerid = autolecturer.PeopleID.ToString();
                ViewBag.AutoLecturer = lecturerid;
                LecturerTimesController lttcontroller = new LecturerTimesController();
                lttcontroller.AddNewTimeAndNewMonth(lecturerid, date);

            }
            else
            {
                var listlecturer = db.People.Where(p => p.Role.Role1 == "Lecturer").ToList();
                Session["checklecturer"] = new List<TeacherDoNotTeach>();
                List<TeacherDoNotTeach> checklecturer = Session["checklecturer"] as List<TeacherDoNotTeach>;
                foreach (var lecturer in listlecturer)
                {
                    var x = lecturer.PeopleID;
                    var result = usedroom.SingleOrDefault(c => c.Class.PeopleID == x);
                    var getif = db.People.SingleOrDefault(c => c.PeopleID == x);
                    if (result == null)
                    {
                        TeacherDoNotTeach ifl = new TeacherDoNotTeach()
                        {
                            PeopleID = getif.PeopleID,
                            Name = getif.Name,
                            RoleID = getif.RoleID,
                            RoleName = getif.Role.Role1,
                        };
                        checklecturer.Add(ifl);
                    }
                }
                var autolecturer = checklecturer.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                var lecturerid = autolecturer.PeopleID.ToString();
                ViewBag.AutoLecturer = lecturerid;
                LecturerTimesController lttcontroller = new LecturerTimesController();
                lttcontroller.AddNewTimeAndNewMonth(lecturerid,date);
            }
            return View();
        }

        public ActionResult CreateAutoAllSlot(DateTime datestr, DateTime dateend, int numberofslot)
        {
            var slot = db.Slots;
            TimeSpan count = dateend.Subtract(datestr);
            StringBuilder sb = new StringBuilder();
            var numberofroom = db.Rooms.ToList();
            if (numberofslot > numberofroom.Count())
            {
                TempData["NoClass"] = "<script>alert('The Input room if larger than the number of the rooms<br />Please input again, the input must not be larger than " + numberofroom.Count() + "');</script>";
            }
            else
            {
                for (int i = 0; i <= count.Days; i++)
                {
                    foreach (var oneslot in slot)
                    {
                        for (int u = 0; u < numberofslot; u++)
                        {
                            DateTime trythis = datestr.AddDays(+i).Date;
                            AutoRoom(trythis, oneslot.SlotID);
                            AutoLesson(trythis, oneslot.SlotID);
                            AutoLecturer(trythis, oneslot.SlotID);

                            // Create New class with lecturer, lesson not dupicate in slot
                            var idclass = RandomString(15, true);
                            db = new ModelContext1();
                            Class newclass = new Class();
                            newclass.ClassID = idclass;
                            newclass.LessonID = ViewBag.AutoLesson;
                            newclass.PeopleID = ViewBag.AutoLecturer;
                            newclass.QuantityMAX = 20;
                            newclass.QuantityMIN = 3;
                            newclass.ClassName = ViewBag.AutoLessonName;
                            db.Classes.Add(newclass);
                            db.SaveChanges();


                            db = new ModelContext1();
                            UsingRoom usr = new UsingRoom();
                            var checkroomUsingRoom = db.UsingRooms.Where(c => c.SlotID == oneslot.SlotID && c.Date == trythis);
                            usr.RoomID = ViewBag.AutoRoom;
                            usr.SlotID = oneslot.SlotID;
                            usr.Date = trythis;
                            usr.DayOfWeek = trythis.DayOfWeek.ToString();
                            usr.ClassID = idclass;
                            usr.QuatityRegister = 0;
                            usr.UseFor = "Teaching";
                            usr.Status = "Just Create";
                            db.UsingRooms.Add(usr);
                            db.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("IndexTest", "UsingRooms");
        }


        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsingRoom usingRoom = db.UsingRooms.Find(id);
            if (usingRoom == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "LessonID", usingRoom.ClassID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RommID", "Floor", usingRoom.RoomID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotID", "SlotName", usingRoom.SlotID);
            return View(usingRoom);
        }

        // POST: UsingRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsingRoomID,RoomID,SlotID,Date,DayOfWeek,ClassID,UseFor,Notes")] UsingRoom usingRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usingRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "LessonID", usingRoom.ClassID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RommID", "Floor", usingRoom.RoomID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotID", "SlotName", usingRoom.SlotID);
            return View(usingRoom);
            //return PartialView("IndexGet", new { slot = usingRoom.SlotID, date = usingRoom.Date });
            //return this.PartialView("../UsingRooms/IndexGet", new { slot = usingRoom.SlotID, date = usingRoom.Date });
            //return RedirectTo
        }

        // GET: UsingRooms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsingRoom usingRoom = db.UsingRooms.Find(id);
            if (usingRoom == null)
            {
                return HttpNotFound();
            }
            return View(usingRoom);
        }

        // POST: UsingRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UsingRoom usingRoom = db.UsingRooms.Find(id);
            db.UsingRooms.Remove(usingRoom);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult CustomeCreate(DateTime datestr1, string check)
        {
            var numberofroom = db.Rooms.Count();
            ViewBag.numberofrooms = numberofroom;
            Session["firstday"] = null;
            Session["Secondday"] = null;
            Session["Thirdday"] = null;
            Session["Fourthday"] = null;
            Session["Fifthday"] = null;
            Session["Sixthday"] = null;
            Session["Seventhday"] = null;
            Session["DayOfWeekfirstday"] = null;
            Session["DayOfWeekSecondday"] = null;
            Session["DayOfWeekThirdday"] = null;
            Session["DayOfWeekFourthday"] = null;
            Session["DayOfWeekFifthday"] = null;
            Session["DayOfWeekSixthday"] = null;
            Session["DayOfWeekSeventhday"] = null;
            Session["firstday"] = datestr1.ToShortDateString();
            Session["DayOfWeekfirstday"] = datestr1.DayOfWeek;
            Session["Secondday"] = datestr1.AddDays(+1).Date.ToShortDateString();
            Session["DayOfWeekSecondday"] = datestr1.AddDays(+1).Date.DayOfWeek;
            Session["Thirdday"] = datestr1.AddDays(+2).Date.ToShortDateString();
            Session["DayOfWeekThirdday"] = datestr1.AddDays(+2).Date.DayOfWeek;
            Session["Fourthday"] = datestr1.AddDays(+3).Date.ToShortDateString();
            Session["DayOfWeekFourthday"] = datestr1.AddDays(+3).Date.DayOfWeek;
            Session["Fifthday"] = datestr1.AddDays(+4).Date.ToShortDateString();
            Session["DayOfWeekFifthday"] = datestr1.AddDays(+4).Date.DayOfWeek;
            Session["Sixthday"] = datestr1.AddDays(+5).Date.ToShortDateString();
            Session["DayOfWeekSixthday"] = datestr1.AddDays(+5).Date.DayOfWeek;
            Session["Seventhday"] = datestr1.AddDays(+6).Date.ToShortDateString();
            Session["DayOfWeekSeventhday"] = datestr1.AddDays(+6).Date.DayOfWeek;
            //check for first day
            var checkslot1fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT01");
            var checkslot2fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT02");
            var checkslot3fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT03");
            var checkslot4fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT04");
            var checkslot5fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT05");
            var checkslot6fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT06");
            var checkslot7fd = db.UsingRooms.Where(c => c.Date == datestr1 && c.SlotID == "SLOT07");
            ViewBag.slot1infd = checkslot1fd.Count();
            ViewBag.slot2infd = checkslot2fd.Count();
            ViewBag.slot3infd = checkslot3fd.Count();
            ViewBag.slot4infd = checkslot4fd.Count();
            ViewBag.slot5infd = checkslot5fd.Count();
            ViewBag.slot6infd = checkslot6fd.Count();
            ViewBag.slot7infd = checkslot7fd.Count();
            //check for second day
            DateTime date1 = datestr1.AddDays(+1).Date;
            var checkslot1snd = db.UsingRooms.Where(c => c.Date == date1 && c.SlotID == "SLOT01");
            var checkslot2snd = db.UsingRooms.Where(c => c.Date == date1.Date && c.SlotID == "SLOT02");
            var checkslot3snd = db.UsingRooms.Where(c => c.Date == date1.Date && c.SlotID == "SLOT03");
            var checkslot4snd = db.UsingRooms.Where(c => c.Date == date1.Date && c.SlotID == "SLOT04");
            var checkslot5snd = db.UsingRooms.Where(c => c.Date == date1.Date && c.SlotID == "SLOT05");
            var checkslot6snd = db.UsingRooms.Where(c => c.Date == date1.Date && c.SlotID == "SLOT06");
            var checkslot7snd = db.UsingRooms.Where(c => c.Date == date1.Date && c.SlotID == "SLOT07");
            ViewBag.slot1insnd = checkslot1snd.Count();
            ViewBag.slot2insnd = checkslot2snd.Count();
            ViewBag.slot3insnd = checkslot3snd.Count();
            ViewBag.slot4insnd = checkslot4snd.Count();
            ViewBag.slot5insnd = checkslot5snd.Count();
            ViewBag.slot6insnd = checkslot6snd.Count();
            ViewBag.slot7insnd = checkslot7snd.Count();
            //check for third day
            DateTime date2 = datestr1.AddDays(+2).Date;
            var checkslot1thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT01");
            var checkslot2thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT02");
            var checkslot3thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT03");
            var checkslot4thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT04");
            var checkslot5thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT05");
            var checkslot6thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT06");
            var checkslot7thrd = db.UsingRooms.Where(c => c.Date == date2 && c.SlotID == "SLOT07");
            ViewBag.slot1inthrd = checkslot1thrd.Count();
            ViewBag.slot2inthrd = checkslot2thrd.Count();
            ViewBag.slot3inthrd = checkslot3thrd.Count();
            ViewBag.slot4inthrd = checkslot4thrd.Count();
            ViewBag.slot5inthrd = checkslot5thrd.Count();
            ViewBag.slot6inthrd = checkslot6thrd.Count();
            ViewBag.slot7inthrd = checkslot7thrd.Count();
            //check for fourth day
            DateTime date3 = datestr1.AddDays(+3).Date;
            var checkslot1frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT01");
            var checkslot2frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT02");
            var checkslot3frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT03");
            var checkslot4frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT04");
            var checkslot5frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT05");
            var checkslot6frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT06");
            var checkslot7frth = db.UsingRooms.Where(c => c.Date == date3 && c.SlotID == "SLOT07");
            ViewBag.slot1infrth = checkslot1frth.Count();
            ViewBag.slot2infrth = checkslot2frth.Count();
            ViewBag.slot3infrth = checkslot3frth.Count();
            ViewBag.slot4infrth = checkslot4frth.Count();
            ViewBag.slot5infrth = checkslot5frth.Count();
            ViewBag.slot6infrth = checkslot6frth.Count();
            ViewBag.slot7infrth = checkslot7frth.Count();
            //check for fifth day
            DateTime date4 = datestr1.AddDays(+4).Date;
            var checkslot1ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT01");
            var checkslot2ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT02");
            var checkslot3ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT03");
            var checkslot4ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT04");
            var checkslot5ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT05");
            var checkslot6ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT06");
            var checkslot7ffth = db.UsingRooms.Where(c => c.Date == date4 && c.SlotID == "SLOT07");
            ViewBag.slot1inffth = checkslot1ffth.Count();
            ViewBag.slot2inffth = checkslot2ffth.Count();
            ViewBag.slot3inffth = checkslot3ffth.Count();
            ViewBag.slot4inffth = checkslot4ffth.Count();
            ViewBag.slot5inffth = checkslot5ffth.Count();
            ViewBag.slot6inffth = checkslot6ffth.Count();
            ViewBag.slot7inffth = checkslot7ffth.Count();
            //check for sixth day
            DateTime date5 = datestr1.AddDays(+5).Date;
            var checkslot1sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT01");
            var checkslot2sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT02");
            var checkslot3sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT03");
            var checkslot4sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT04");
            var checkslot5sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT05");
            var checkslot6sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT06");
            var checkslot7sth = db.UsingRooms.Where(c => c.Date == date5 && c.SlotID == "SLOT07");
            ViewBag.slot1insth = checkslot1sth.Count();
            ViewBag.slot2insth = checkslot2sth.Count();
            ViewBag.slot3insth = checkslot3sth.Count();
            ViewBag.slot4insth = checkslot4sth.Count();
            ViewBag.slot5insth = checkslot5sth.Count();
            ViewBag.slot6insth = checkslot6sth.Count();
            ViewBag.slot7insth = checkslot7sth.Count();
            //check for seventh day
            DateTime date6 = datestr1.AddDays(+6).Date;
            var checkslot1svth = db.UsingRooms.Where(c => c.Date == date6 && c.SlotID == "SLOT01");
            var checkslot2svth = db.UsingRooms.Where(c => c.Date == date6 && c.SlotID == "SLOT02");
            var checkslot3svth = db.UsingRooms.Where(c => c.Date == date6 && c.SlotID == "SLOT03");
            var checkslot4svth = db.UsingRooms.Where(c => c.Date == date6 && c.SlotID == "SLOT04");
            var checkslot5svth = db.UsingRooms.Where(c => c.Date == date6.Date && c.SlotID == "SLOT05");
            var checkslot6svth = db.UsingRooms.Where(c => c.Date == date6.Date && c.SlotID == "SLOT06");
            var checkslot7svth = db.UsingRooms.Where(c => c.Date == date6.Date && c.SlotID == "SLOT07");
            ViewBag.slot1insvth = checkslot1svth.Count();
            ViewBag.slot2insvth = checkslot2svth.Count();
            ViewBag.slot3insvth = checkslot3svth.Count();
            ViewBag.slot4insvth = checkslot4svth.Count();
            ViewBag.slot5insvth = checkslot5svth.Count();
            ViewBag.slot6insvth = checkslot6svth.Count();
            ViewBag.slot7insvth = checkslot7svth.Count();
            if (check == "a")
            {
                return PartialView("../UsingRooms/CustomeCreatePartial");

            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult CustomeCreate(int? slot1fd, int? slot1snd, int? slot1thrd, int? slot1frth, int? slot1ffth, int? slot1sth, int? slot1svth, int? slot2fd, int? slot2snd, int? slot2thrd, int? slot2frth, int? slot2ffth, int? slot2sth, int? slot2svth, int? slot3fd, int? slot3snd, int? slot3thrd, int? slot3frth, int? slot3ffth, int? slot3sth, int? slot3svth, int? slot4fd, int? slot4snd, int? slot4thrd, int? slot4frth, int? slot4ffth, int? slot4sth, int? slot4svth, int? slot5fd, int? slot5snd, int? slot5thrd, int? slot5frth, int? slot5ffth, int? slot5sth, int? slot5svth, int? slot6fd, int? slot6snd, int? slot6thrd, int? slot6frth, int? slot6ffth, int? slot6sth, int? slot6svth, int? slot7fd, int? slot7snd, int? slot7thrd, int? slot7frth, int? slot7ffth, int? slot7sth, int? slot7svth)
        {
            string x = Session["firstday"].ToString();
            DateTime date = Convert.ToDateTime(x);
            var numberofroom = db.Rooms.Count();
            Boolean flag1 = false;
            Boolean flag2 = false;
            Boolean flag3 = false;
            Boolean flag4 = false;
            Boolean flag5 = false;
            Boolean flag6 = false;
            Boolean flag7 = false;
            for (int i = 0; i < 7; i++)
            {
                DateTime dateend = date.AddDays(+i);
                TimeSpan count = dateend.Subtract(date);
                DateTime trythis1 = date.AddDays(+i).Date;
                var checkslot1 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT01");
                var checkslot2 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT02");
                var checkslot3 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT03");
                var checkslot4 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT04");
                var checkslot5 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT05");
                var checkslot6 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT06");
                var checkslot7 = db.UsingRooms.Where(c => c.Date == trythis1 && c.SlotID == "SLOT07");
                if (count.Days == 0)
                {
                    if (slot1fd > numberofroom || slot1fd + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the First day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2fd > numberofroom || slot2fd + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in First day is large than the numebr of room');</script>";
                    }
                    else if (slot3fd > numberofroom || slot3fd + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in First day is large than the numebr of room');</script>";
                    }
                    else if (slot4fd > numberofroom || slot4fd + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in First day is large than the numebr of room');</script>";
                    }
                    else if (slot5fd > numberofroom || slot5fd + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in First day is large than the numebr of room');</script>";
                    }
                    else if (slot6fd > numberofroom || slot6fd + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in First day is large than the numebr of room');</script>";
                    }
                    else if (slot7fd > numberofroom || slot7fd + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in First day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag1 = true;
                    }
                }
                else if (count.Days == 1)
                {
                    if (slot1snd > numberofroom || slot1snd + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the Second day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2snd > numberofroom || slot2snd + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in Second day is large than the numebr of room');</script>";
                    }
                    else if (slot3snd > numberofroom || slot3snd + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in Second day is large than the numebr of room');</script>";
                    }
                    else if (slot4snd > numberofroom || slot4snd + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in Second day is large than the numebr of room');</script>";
                    }
                    else if (slot5snd > numberofroom || slot5snd + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in Second day is large than the numebr of room');</script>";
                    }
                    else if (slot6snd > numberofroom || slot6snd + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in Second day is large than the numebr of room');</script>";
                    }
                    else if (slot7snd > numberofroom || slot7snd + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in Second day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag2 = true;
                    }
                }
                else if (count.Days == 2)
                {
                    if (slot1thrd > numberofroom || slot1thrd + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the Third day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2thrd > numberofroom || slot2thrd + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in Third day is large than the numebr of room');</script>";
                    }
                    else if (slot3thrd > numberofroom || slot3thrd + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in Third day is large than the numebr of room');</script>";
                    }
                    else if (slot4thrd > numberofroom || slot4thrd + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in Third day is large than the numebr of room');</script>";
                    }
                    else if (slot5thrd > numberofroom || slot5thrd + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in Third day is large than the numebr of room');</script>";
                    }
                    else if (slot6thrd > numberofroom || slot6thrd + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in Third day is large than the numebr of room');</script>";
                    }
                    else if (slot7thrd > numberofroom || slot7thrd + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in Third day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag3 = true;
                    }
                }
                else if (count.Days == 3)
                {
                    if (slot1frth > numberofroom || slot1frth + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the Fourth day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2frth > numberofroom || slot2frth + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in Fourth day is large than the numebr of room');</script>";
                    }
                    else if (slot3frth > numberofroom || slot3frth + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in Fourth day is large than the numebr of room');</script>";
                    }
                    else if (slot4frth > numberofroom || slot4frth + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in Fourth day is large than the numebr of room');</script>";
                    }
                    else if (slot5frth > numberofroom || slot5frth + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in Fourth day is large than the numebr of room');</script>";
                    }
                    else if (slot6frth > numberofroom || slot6frth + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in Fourth day is large than the numebr of room');</script>";
                    }
                    else if (slot7frth > numberofroom || slot7frth + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in Fourth day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag4 = true;
                    }
                }
                else if (count.Days == 4)
                {
                    if (slot1ffth > numberofroom || slot1ffth + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the Fifth day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2ffth > numberofroom || slot2ffth + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in Fifth day is large than the numebr of room');</script>";
                    }
                    else if (slot3ffth > numberofroom || slot3ffth + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in Fifth day is large than the numebr of room');</script>";
                    }
                    else if (slot4ffth > numberofroom || slot4ffth + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in Fifth day is large than the numebr of room');</script>";
                    }
                    else if (slot5ffth > numberofroom || slot5ffth + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in Fifth day is large than the numebr of room');</script>";
                    }
                    else if (slot6ffth > numberofroom || slot6ffth + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in Fifth day is large than the numebr of room');</script>";
                    }
                    else if (slot7ffth > numberofroom || slot7ffth + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in Fifth day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag5 = true;
                    }
                }
                else if (count.Days == 5)
                {
                    if (slot1sth > numberofroom || slot1sth + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the Sixth day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2sth > numberofroom || slot2sth + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in Sixth day is large than the numebr of room');</script>";
                    }
                    else if (slot3sth > numberofroom || slot3sth + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in Sixth day is large than the numebr of room');</script>";
                    }
                    else if (slot4sth > numberofroom || slot4sth + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in Sixth day is large than the numebr of room');</script>";
                    }
                    else if (slot5sth > numberofroom || slot5sth + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in Sixth day is large than the numebr of room');</script>";
                    }
                    else if (slot6sth > numberofroom || slot6sth + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in Sixth day is large than the numebr of room');</script>";
                    }
                    else if (slot7sth > numberofroom || slot7sth + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in Sixth day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag6 = true;
                    }
                }
                else if (count.Days == 6)
                {
                    if (slot1svth > numberofroom || slot1svth + checkslot1.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('The input in Slot 1 of the Seventh day must not be larger than the number of the rooms or the contain rooms plus with the input must not be larger than the number of the rooms ');</script>";
                    }
                    else if (slot2svth > numberofroom || slot2svth + checkslot2.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 2 in Seventh day is large than the numebr of room');</script>";
                    }
                    else if (slot3svth > numberofroom || slot3svth + checkslot3.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 3 in Seventh day is large than the numebr of room');</script>";
                    }
                    else if (slot4svth > numberofroom || slot4svth + checkslot4.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 4 in Seventh day is large than the numebr of room');</script>";
                    }
                    else if (slot5svth > numberofroom || slot5svth + checkslot5.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 5 in Seventh day is large than the numebr of room');</script>";
                    }
                    else if (slot6svth > numberofroom || slot6svth + checkslot6.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 6 in Seventh day is large than the numebr of room');</script>";
                    }
                    else if (slot7svth > numberofroom || slot7svth + checkslot7.Count() > numberofroom)
                    {
                        TempData["NoClass"] = "<script>alert('Slot 7 in Seventh day is large than the numebr of room');</script>";
                    }
                    else
                    {
                        flag7 = true;
                    }
                }

            }
            if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true && flag7 == true)
            {
                for (int i = 0; i < 7; i++)
                {
                    DateTime dateend = date.AddDays(+i);
                    TimeSpan count = dateend.Subtract(date);
                    DateTime trythis1 = date.AddDays(+i).Date;
                    if (count.Days == 0)
                    {
                        if (slot1fd != null)
                        {
                            for (int u = 0; u < slot1fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2fd != null)
                        {
                            for (int u = 0; u < slot2fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3fd != null)
                        {
                            for (int u = 0; u < slot3fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4fd != null)
                        {
                            for (int u = 0; u < slot4fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5fd != null)
                        {
                            for (int u = 0; u < slot5fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6fd != null)
                        {
                            for (int u = 0; u < slot6fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7fd != null)
                        {
                            for (int u = 0; u < slot7fd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                    else if (count.Days == 1)
                    {
                        if (slot1snd != null)
                        {
                            for (int u = 0; u < slot1snd; u++)
                            {

                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2snd != null)
                        {
                            for (int u = 0; u < slot2snd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3snd != null)
                        {
                            for (int u = 0; u < slot3snd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4snd != null)
                        {
                            for (int u = 0; u < slot4snd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5snd != null)
                        {
                            for (int u = 0; u < slot5snd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6snd != null)
                        {
                            for (int u = 0; u < slot6snd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7snd != null)
                        {
                            for (int u = 0; u < slot7snd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                    else if (count.Days == 2)
                    {
                        if (slot1thrd != null)
                        {
                            for (int u = 0; u < slot1thrd; u++)
                            {

                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2thrd != null)
                        {
                            for (int u = 0; u < slot2thrd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3thrd != null)
                        {
                            for (int u = 0; u < slot3thrd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4thrd != null)
                        {
                            for (int u = 0; u < slot4thrd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5thrd != null)
                        {
                            for (int u = 0; u < slot5thrd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6thrd != null)
                        {
                            for (int u = 0; u < slot6thrd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7thrd != null)
                        {
                            for (int u = 0; u < slot7thrd; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                    else if (count.Days == 3)
                    {
                        if (slot1frth != null)
                        {
                            for (int u = 0; u < slot1frth; u++)
                            {

                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2frth != null)
                        {
                            for (int u = 0; u < slot2frth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3frth != null)
                        {
                            for (int u = 0; u < slot3frth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4frth != null)
                        {
                            for (int u = 0; u < slot4frth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5frth != null)
                        {
                            for (int u = 0; u < slot5frth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6frth != null)
                        {
                            for (int u = 0; u < slot6frth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7frth != null)
                        {
                            for (int u = 0; u < slot7frth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                    else if (count.Days == 4)
                    {
                        if (slot1ffth != null)
                        {
                            for (int u = 0; u < slot1ffth; u++)
                            {

                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2ffth != null)
                        {
                            for (int u = 0; u < slot2ffth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3ffth != null)
                        {
                            for (int u = 0; u < slot3ffth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4ffth != null)
                        {
                            for (int u = 0; u < slot4ffth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5ffth != null)
                        {
                            for (int u = 0; u < slot5ffth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6ffth != null)
                        {
                            for (int u = 0; u < slot6ffth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7ffth != null)
                        {
                            for (int u = 0; u < slot7ffth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                    else if (count.Days == 5)
                    {
                        if (slot1sth != null)
                        {
                            for (int u = 0; u < slot1sth; u++)
                            {

                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2sth != null)
                        {
                            for (int u = 0; u < slot2sth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3sth != null)
                        {
                            for (int u = 0; u < slot3sth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4sth != null)
                        {
                            for (int u = 0; u < slot4sth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5sth != null)
                        {
                            for (int u = 0; u < slot5sth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6sth != null)
                        {
                            for (int u = 0; u < slot6sth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7sth != null)
                        {
                            for (int u = 0; u < slot7sth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                    else if (count.Days == 6)
                    {
                        if (slot1svth != null)
                        {
                            for (int u = 0; u < slot1svth; u++)
                            {

                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT01");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT01", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT01", trythis);
                                }
                            }
                        }
                        if (slot2svth != null)
                        {
                            for (int u = 0; u < slot2svth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT02");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT02", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT02", trythis);
                                }
                            }
                        }
                        if (slot3svth != null)
                        {
                            for (int u = 0; u < slot3svth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT03");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT03", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT03", trythis);
                                }
                            }
                        }
                        if (slot4svth != null)
                        {
                            for (int u = 0; u < slot4svth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT04");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT04", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT04", trythis);
                                }
                            }
                        }
                        if (slot5svth != null)
                        {
                            for (int u = 0; u < slot5svth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT05");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT05", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT05", trythis);
                                }
                            }
                        }
                        if (slot6svth != null)
                        {
                            for (int u = 0; u < slot6svth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT06");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT06", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT06", trythis);
                                }
                            }
                        }
                        if (slot7svth != null)
                        {
                            for (int u = 0; u < slot7svth; u++)
                            {
                                DateTime trythis = date.AddDays(+i).Date;
                                var usedroom = db.UsingRooms.Where(c => c.Date == trythis && c.SlotID == "SLOT07");
                                if (usedroom == null)
                                {
                                    CreateURWithEmptyBefore("SLOT07", trythis);
                                }
                                else
                                {
                                    CreateURHasNoEmptyBeofre("SLOT07", trythis);
                                }
                            }
                        }
                    }
                }
            }

            return RedirectToAction("CustomeCreate", "UsingRooms", new { datestr1 = date });
        }

        public ActionResult CreateURWithEmptyBefore(string slot, DateTime date)
        {
            var listlessons = db.Lessons.ToList();
            var listlecturer = db.People.Where(c => c.Role.Role1 == "Lecturer");
            var listroom = db.Rooms.ToList();
            var autoroom = listroom.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            var autolesson = listlessons.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            var autolecturer = listlecturer.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            var idclass = RandomString(15, true);
            db = new ModelContext1();
            Class newclass = new Class();
            newclass.ClassID = idclass;
            newclass.LessonID = autolesson.LessonID;
            newclass.PeopleID = autolecturer.PeopleID;
            newclass.QuantityMAX = 20;
            newclass.QuantityMIN = 3;
            newclass.ClassName = autolesson.LessonName;
            db.Classes.Add(newclass);
            db.SaveChanges();

            db = new ModelContext1();
            UsingRoom usr = new UsingRoom();
            var checkroomUsingRoom = db.UsingRooms.Where(c => c.SlotID == slot && c.Date == date);
            usr.RoomID = autoroom.RommID;
            usr.SlotID = slot;
            usr.Date = date;
            usr.DayOfWeek = date.DayOfWeek.ToString();
            usr.ClassID = idclass;
            usr.QuatityRegister = 0;
            usr.UseFor = "Teaching";
            usr.Status = "Just Create";
            db.UsingRooms.Add(usr);
            db.SaveChanges();
            return View();
        }

        public ActionResult CreateURHasNoEmptyBeofre(string slot, DateTime date)
        {
            AutoLesson(date, slot);
            AutoLecturer(date, slot);
            AutoRoom(date, slot);

            var idclass = RandomString(15, true);
            db = new ModelContext1();
            Class newclass = new Class();
            newclass.ClassID = idclass;
            newclass.LessonID = ViewBag.AutoLesson;
            newclass.PeopleID = ViewBag.AutoLecturer;
            newclass.QuantityMAX = 20;
            newclass.QuantityMIN = 3;
            newclass.ClassName = ViewBag.AutoLessonName;
            db.Classes.Add(newclass);
            db.SaveChanges();

            db = new ModelContext1();
            UsingRoom usr = new UsingRoom();
            usr.RoomID = ViewBag.AutoRoom;
            usr.SlotID = slot;
            usr.Date = date;
            usr.DayOfWeek = date.DayOfWeek.ToString();
            usr.ClassID = idclass;
            usr.QuatityRegister = 0;
            usr.UseFor = "Teaching";
            usr.Status = "Just Create";
            db.UsingRooms.Add(usr);
            db.SaveChanges();
            return View();
        }
        public RedirectToRouteResult EnalbleForOneUsingRoom(int id)
        {

            var setUSR = db.UsingRooms.Find(id);
            setUSR.Status = "Enable";
            db.Entry(setUSR).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult DisableForOneUsingRoom(int id)
        {
            var setUSR = db.UsingRooms.Find(id);
            setUSR.Status = "Disable";
            db.Entry(setUSR).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
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
