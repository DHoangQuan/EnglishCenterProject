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
    public class PeopleController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        public ActionResult Login(Person loginPerson)
        {
            try
            {
                var acc = db.People.SingleOrDefault(a => a.PeopleID.Equals(loginPerson.PeopleID));
                if (acc == null)
                {
                    ModelState.AddModelError("", "Invalid Phone");
                }
                else
                {
                    if (acc.Password.Equals(loginPerson.Password))
                    {
                        if(acc.Image == null || acc.Image == "")
                        {
                            Session["ImagePerson"] = "null";
                        }else
                        {
                            Session["ImagePerson"] = acc.Image;
                        }
                        Session["PersonName"] = acc.Name;
                        Session["PersonID"] = acc.PeopleID;
                        Session["RoleID"] = acc.RoleID;
                        Session["RoleName"] = acc.Role.Role1;
                        return RedirectToAction("Index", "Skills");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Password");
                    }
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(loginPerson);
        }

        public ActionResult Logout()
        {
            Session["ImagePerson"] = null;
            Session["PersonName"] = null;
            Session["PersonID"] = null;
            Session["RoleID"] = null;
            Session["RoleName"] = null;
            return RedirectToAction("IndexClient", "Contents");

        }

        public ActionResult DetailsPartial(string id)
        {
            Person person = db.People.Find(id);
            
            return PartialView("../People/DetailsPartial", person);
        }

        public ActionResult EditAva(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeopleID = new SelectList(db.Authentications, "PeopleID", "EditAuthetication", person.PeopleID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role1", person.RoleID);
            return PartialView("../People/EditAva", person);
        }
        [HttpPost]
        public ActionResult EditAva(Person person)
        {
            Session["ImagePerson"] = null;
            
            db.Entry(person).State = EntityState.Modified;
            db.SaveChanges();
            Session["ImagePerson"] = person.Image;
            return RedirectToAction("Details", "People", new { id = person.PeopleID});
        }
        // GET: People
        public ActionResult Index()
        {
            var people = db.People.Include(p => p.Authentication).Include(p => p.Role);
            return View(people.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(string id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            Session["DetailsPersonID"] = null;
            Session["DetailsPersonName"] = null;
            Session["DetailsPersonID"] = id;
            Session["DetailsPersonName"] = person.Name;
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        public ActionResult DetailsForAdminRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.PeopleID = new SelectList(db.Authentications, "PeopleID", "EditAuthetication");
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role1");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PeopleID,Name,Phone,DoB,Gender,Email,Active,Password,Address,Image,RoleID")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeopleID = new SelectList(db.Authentications, "PeopleID", "EditAuthetication", person.PeopleID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role1", person.RoleID);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeopleID = new SelectList(db.Authentications, "PeopleID", "EditAuthetication", person.PeopleID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role1", person.RoleID);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PeopleID,Name,Phone,DoB,Gender,Email,Active,Password,Address,Image,RoleID")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeopleID = new SelectList(db.Authentications, "PeopleID", "EditAuthetication", person.PeopleID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role1", person.RoleID);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
