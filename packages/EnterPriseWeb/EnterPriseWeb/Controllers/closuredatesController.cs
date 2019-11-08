using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnterPriseWeb.Models;

namespace EnterPriseWeb.Controllers
{
    public class closuredatesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: closuredates
        public ActionResult Index()
        {
            return View(db.closuredates.ToList());
        }

        // GET: closuredates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            closuredate closuredate = db.closuredates.Find(id);
            if (closuredate == null)
            {
                return HttpNotFound();
            }
            return View(closuredate);
        }

        // GET: closuredates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: closuredates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Year,Closure_date,Final_Closure_date")] closuredate closuredate, string Year)
        {
            if (ModelState.IsValid)
            {
                int Ye = Convert.ToInt32(Year);
                closuredate.Year = Ye;
                db.closuredates.Add(closuredate);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(closuredate);
        }

        // GET: closuredates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            closuredate closuredate = db.closuredates.Find(id);
            if (closuredate == null)
            {
                return HttpNotFound();
            }
            return View(closuredate);
        }

        // POST: closuredates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Year,Closure_date,Final_Closure_date")] closuredate closuredate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(closuredate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(closuredate);
        }

        // GET: closuredates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            closuredate closuredate = db.closuredates.Find(id);
            if (closuredate == null)
            {
                return HttpNotFound();
            }
            return View(closuredate);
        }

        // POST: closuredates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            closuredate closuredate = db.closuredates.Find(id);
            db.closuredates.Remove(closuredate);
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
