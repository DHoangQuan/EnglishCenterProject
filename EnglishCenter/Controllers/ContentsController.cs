using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using EnglishCenter.Models;
using System.Text;
using PusherServer;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;


namespace EnglishCenter.Controllers
{
    public class ContentsController : Controller
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
        public ActionResult Comments(int? id)
        {        
            db.Configuration.ProxyCreationEnabled = false;
            var comments = db.Comments.Where(x => x.content_id == id).ToArray();
            return Json(comments, JsonRequestBehavior.AllowGet);

            //db.Configuration.ProxyCreationEnabled = false;
            //List<Comment> comments = db.Comments.Where(x => x.content_id == id).ToList();
            //foreach (Comment com in comments)
            //{
            //    Person person = db.People.Where(x => x.PeopleID == com.PeopleID).FirstOrDefault();
            //    com.Person = person;
            //}
            //var json = JsonConvert.SerializeObject(new { comments = comments });
            //return Json(json, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> Comment(Comment data)
        {
            data.Image = "/Uploads/images/IEL.jpg";
            data.Name = "Quân";
            data.comment_date = DateTime.Now;
            data.PeopleID = "P01";
            db.Comments.Add(data);
            db.SaveChanges();
            var options = new PusherOptions();
            options.Cluster = "ap1";
            var pusher = new Pusher("888322", "8ce5a959f6c9469e2d20", "f492271da2057c74ed07", options);
            ITriggerResult result = await pusher.TriggerAsync("asp_channel", "asp_event", data);
            return Content("ok");
        }

        // GET: Contents
        public ActionResult IndexTopicPostClient(string id, int? page)
        {
            Session["TopicpostHot"] = null;
            Session["CheckslideInIndexTopic"] = null;

            var slide11 = db.Contents.Where(c => c.status == "Slide" && c.TopicPostID == id);
            if (slide11.Count() == 0 || slide11 == null)
            {
                Session["CheckslideInIndexTopic"] = "0";
            }
            else
            {
                Session["CheckslideInIndexTopic"] = "1";
            }
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var toppicpost = db.Contents.Where(c => c.TopicPostID == id && c.status != "Disable");

            Session["TopicpostHot"] = id.ToString();
            return View(toppicpost.OrderByDescending(x => x.date_post).ToList().ToPagedList(pageIndex, pageSize));
        }
        public ActionResult SlideHotInTopic()
        {
            string x = Session["TopicpostHot"].ToString();
            var slide1 = db.Contents.Where(c => c.status == "Slide" && c.TopicPostID == x);
            return PartialView("../Contents/SlideHotInTopic", slide1);
        }
        public ActionResult SlideAll()
        {
            var slide = db.Contents.Where(c => c.status == "Slide");
            return PartialView("../Contents/SlideAll", slide);
        }
        public ActionResult IndexClient(int? page)
        {
            Session["Checkslide"] = null;
            var slide = db.Contents.Where(c => c.status == "Slide");
            if (slide.Count() == 0 || slide == null)
            {
                Session["Checkslide"] = "0";
            }
            else
            {
                Session["Checkslide"] = "1";
            }
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            Session["TopicpostHot"] = null;
            var contents = db.Contents.Where(w => w.status != "Disable");
            return View(contents.OrderByDescending(x => x.date_post).ToList().ToPagedList(pageIndex, pageSize));
        }
        public RedirectToRouteResult Disable(int id)
        {
            var setcontent = db.Contents.Find(id);
            setcontent.status = "Disable";
            db.Entry(setcontent).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Slide(int id)
        {
            var setcontent = db.Contents.Find(id);
            setcontent.status = "Slide";
            db.Entry(setcontent).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Enable(int id)
        {
            var setcontent = db.Contents.Find(id);
            setcontent.status = "Enable";
            db.Entry(setcontent).State = EntityState.Modified;
            db.SaveChanges();
            db = new ModelContext1();
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {

            var contents = db.Contents.Include(c => c.Person).Include(c => c.TopicPost);


            //var orderbydecending = from s in contents orderby s.content_id descending select s;

            return View(contents.OrderByDescending(d => d.date_post).ToList());

            //return View(orderbydecending);

        }

        // GET: Contents/Details/5
        public ActionResult Details(int id)
        {

            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        public ActionResult DetailInClient(int id)
        {

            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents/Create
        public ActionResult Create()
        {
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name");
            ViewBag.TopicPostID = new SelectList(db.TopicPosts, "TopicPostID", "TopicName");
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Content content)
        {
            if (ModelState.IsValid)
            {


                //content.status = "Disable";
                content.status = "Enable";
                content.date_post = DateTime.Now;
                db.Contents.Add(content);
                db.SaveChanges();
                db = new ModelContext1();
            }

            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", content.PeopleID);
            ViewBag.TopicPostID = new SelectList(db.TopicPosts, "TopicPostID", "TopicName", content.TopicPostID);
            return RedirectToAction("Index");
            //return View(content);
        }

        public ActionResult EditRepresentImage(int id)
        {
            
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", content.PeopleID);
            ViewBag.TopicPostID = new SelectList(db.TopicPosts, "TopicPostID", "TopicName", content.TopicPostID);
            return PartialView("../Contents/EditRepresentImage", content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult EditRepresentImage(Content content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", content.PeopleID);
            ViewBag.TopicPostID = new SelectList(db.TopicPosts, "TopicPostID", "TopicName", content.TopicPostID);
            return RedirectToAction("Edit", "Contents", new { id = content.content_id });
        }

        // GET: Contents/Edit/5
        public ActionResult Edit(int id)
        {
            
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", content.PeopleID);
            ViewBag.TopicPostID = new SelectList(db.TopicPosts, "TopicPostID", "TopicName", content.TopicPostID);
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeopleID = new SelectList(db.People, "PeopleID", "Name", content.PeopleID);
            ViewBag.TopicPostID = new SelectList(db.TopicPosts, "TopicPostID", "TopicName", content.TopicPostID);
            return View(content);
        }

        // GET: Contents/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
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
