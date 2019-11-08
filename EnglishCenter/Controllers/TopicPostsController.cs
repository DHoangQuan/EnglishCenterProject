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
    public class TopicPostsController : Controller
    {
        private ModelContext1 db = new ModelContext1();

        // GET: TopicPosts
        
        public ActionResult Index()
        {
            var topicPosts = db.TopicPosts.Include(t => t.Catalogue);
            return View(topicPosts.ToList());
        }
        public ActionResult TopicPostListPinInBarInClient()
        {
            return PartialView(db.TopicPosts.Where(c => c.PinToBar == true).ToList());
        }
        public ActionResult TopicPostListInClient(string id)
        {
            return PartialView(db.TopicPosts.Where(c=>c.CatalogueID ==id).ToList());
        }
        // GET: TopicPosts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicPost topicPost = db.TopicPosts.Find(id);
            if (topicPost == null)
            {
                return HttpNotFound();
            }
            return View(topicPost);
        }

        // GET: TopicPosts/Create
        public ActionResult Create()
        {
            ViewBag.CatalogueID = new SelectList(db.Catalogues, "Catalogue1", "CatalogueName");
            return View();
        }

        // POST: TopicPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicPostID,CatalogueID,TopicName,PinToBar")] TopicPost topicPost)
        {
            if (ModelState.IsValid)
            {
                topicPost.Status = "Disable";
                db.TopicPosts.Add(topicPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatalogueID = new SelectList(db.Catalogues, "Catalogue1", "CatalogueName", topicPost.CatalogueID);
            return View(topicPost);
        }

        // GET: TopicPosts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicPost topicPost = db.TopicPosts.Find(id);
            if (topicPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatalogueID = new SelectList(db.Catalogues, "Catalogue1", "CatalogueName", topicPost.CatalogueID);
            return View(topicPost);
        }

        // POST: TopicPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicPostID,CatalogueID,TopicName,PinToBar")] TopicPost topicPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topicPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatalogueID = new SelectList(db.Catalogues, "Catalogue1", "CatalogueName", topicPost.CatalogueID);
            return View(topicPost);
        }

        // GET: TopicPosts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicPost topicPost = db.TopicPosts.Find(id);
            if (topicPost == null)
            {
                return HttpNotFound();
            }
            return View(topicPost);
        }

        // POST: TopicPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TopicPost topicPost = db.TopicPosts.Find(id);
            db.TopicPosts.Remove(topicPost);
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
