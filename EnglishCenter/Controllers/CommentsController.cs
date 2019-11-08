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
    public class CommentsController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Content).Include(c => c.Person);
            return View(comments.ToList());
        }



        // GET: Comments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        
        

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.content_id = new SelectList(db.Contents, "content_id", "content_title");
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "comment_id,content_id,comment_content,PeopleID,comment_date")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.PeopleID = "P01";
                comment.comment_date = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.content_id = new SelectList(db.Contents, "content_id", "content_title", comment.content_id);
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", comment.PeopleID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.content_id = new SelectList(db.Contents, "content_id", "content_title", comment.content_id);
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", comment.PeopleID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "comment_id,content_id,comment_content,PeopleID,comment_date")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.content_id = new SelectList(db.Contents, "content_id", "content_title", comment.content_id);
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", comment.PeopleID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
