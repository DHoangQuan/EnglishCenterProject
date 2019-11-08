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
    public class QuotesController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        // GET: Quotes
        public ActionResult Index()
        {
            return View(db.Quotes.OrderByDescending(x => x.QuoteID).ToList());
        }
        public RedirectToRouteResult Off(string id)
        {         
            var setquote = db.Quotes.Find(id);
            setquote.Status = "Off";
            db.Entry(setquote).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Set1(string id)
        {
            
            var checkstatus = db.Quotes.FirstOrDefault(c=>c.Status =="1");
            if (checkstatus != null)
            {
                Quote findtoof = db.Quotes.Find(checkstatus.QuoteID);
                findtoof.Status = "Off";
                db.Entry(findtoof).State = EntityState.Modified;
                db.SaveChanges();
                db = new ModelContext1();
            }
            var setquote = db.Quotes.Find(id);
            setquote.Status = "1";
            db.Entry(setquote).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Set2(string id)
        {
            var checkstatus = db.Quotes.FirstOrDefault(c => c.Status == "2");
            if (checkstatus != null)
            {
                Quote findtoof = db.Quotes.Find(checkstatus.QuoteID);
                findtoof.Status = "Off";
                db.Entry(findtoof).State = EntityState.Modified;
                db.SaveChanges();
                db = new ModelContext1();
            }
            var setquote = db.Quotes.Find(id);
            setquote.Status = "2";
            db.Entry(setquote).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public ActionResult ShowQuote1()
        {
            var quote1 = db.Quotes.Where(c=>c.Status == "1");
            //return View(quote1);
            return PartialView("../Quotes/ShowQuote1", quote1);
        }

        public ActionResult ShowQuote2()
        {
            var quote2 = db.Quotes.Where(c => c.Status == "2");
            //return View(quote2);
            return PartialView("../Quotes/ShowQuote2", quote2);
        }

        // GET: Quotes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // GET: Quotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuoteID,Titlle,Author,ContentQuote,Status")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                quote.Status = "Off";
                db.Quotes.Add(quote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quote);
        }

        // GET: Quotes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuoteID,Titlle,Author,ContentQuote,Status")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Quote quote = db.Quotes.Find(id);
            db.Quotes.Remove(quote);
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
