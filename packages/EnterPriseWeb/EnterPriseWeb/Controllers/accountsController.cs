using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnterPriseWeb.Models;
using System.Text;
using System.IO;

namespace EnterPriseWeb.Controllers
{
    public class accountsController : Controller
    {
        private ModelContext db = new ModelContext();
        
        // GET: accounts
        public ActionResult Index()
        {

           
                var accounts = db.accounts.Include(a => a.faculty).Include(a => a.role);
                return View(accounts.ToList());
            

            //return View();
        }
        public ActionResult IndexActivated()
        {
            
                var accounts = db.accounts.Include(a => a.faculty).Include(a => a.role).Where(e => e.active == "Activated");
                return View(accounts.ToList());
            
        }
        public ActionResult IndexLocked()
        {
            
                var accounts = db.accounts.Include(a => a.faculty).Include(a => a.role).Where(e => e.active == "Locked");
                return View(accounts.ToList());
            

        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["accountName"] = null;
            Session["accountID"] = null;
            Session["accountRole"] = null;
            Session["accountRoleName"] = null;
            Session["accountFaculty"] = null;
            return RedirectToAction("Login", "accounts");

        }
        [HttpPost]
        public ActionResult Login(account login)
        {
            try
            {
                var acc = db.accounts.SingleOrDefault(a => a.account_id.Equals(login.account_id));
                Session["accountName"] = acc.account_name;
                Session["accountID"] = acc.account_id;
                Session["accountRole"] = acc.role_id;
                Session["accountRoleName"] = acc.role.role_name;
                Session["accountFaculty"] = acc.falcuty_id;
                if (acc == null)
                {
                    ModelState.AddModelError("", "Invalid ID");
                }
                else
                {
                    if (acc.active.Equals("Locked"))
                    {
                        TempData["msg"] = "<script>alert('Your account is locked');</script>";
                    }
                    else
                    {   
                        if (acc.password.Equals(login.password))
                        {
                            if (acc.role_id == "ST")
                            {
                                return RedirectToAction("Index", "contents");
                            }
                            return RedirectToAction("Index", "accounts");
                        }
                        else
                        {
                            TempData["msg"] = "<script>alert('Invalid Password');</script>";
                            ModelState.AddModelError("", "Invalid Password");
                        }
                    }
                    
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(login);
        }

        // GET: accounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: accounts/Create
        public ActionResult Create()
        {
            ViewBag.falcuty_id = new SelectList(db.faculties, "faculty_id", "faculty_name");
            ViewBag.role_id = new SelectList(db.roles, "role_id", "role_name");
            return View();
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
        // POST: accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public string numberid(int x)
        {
            StringBuilder sb = new StringBuilder();
            if (x < 10000 && x >= 1000)
            {
                sb.Append("0").Append(x.ToString());
            }
            else if (x < 1000 && x >= 100)
            {
                sb.Append("00").Append(x.ToString());
            }
            else if (x < 100 & x >= 10)
            {
                sb.Append("000").Append(x.ToString());
            }
            else if (x < 10)
            {
                sb.Append("0000").Append(x.ToString());
            }
            else
            {
                sb.Append(x.ToString());
            }
            return sb.ToString();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,account_name,password,Dob,phone,email,address,sex,account_date,role_id,avatar,active,falcuty_id")] account account, HttpPostedFileBase file)
        {
            StringBuilder id = new StringBuilder();
            var x = RandomNumber(1, 99999);
            if (file.ContentLength > 0)
            {
                string imgPath = Path.GetFileName(file.FileName);
                string url = Path.Combine(Server.MapPath("~/Avatar/"), imgPath);
                file.SaveAs(url);
                account.avatar = "/Avatar/" + imgPath;
            }
            if (ModelState.IsValid)
            {
                if (Session["accountRole"].ToString() == "MC")
                {
                    id.Append("ST").Append((Session["accountFaculty"]).ToString()).Append(numberid(x).ToString());
                    account.account_id = id.ToString();
                    account.role_id = "ST";
                    account.falcuty_id = Session["accountFaculty"].ToString();
                }
                else if(account.role_id == "ST")
                {
                    id.Append("ST").Append((Session["accountFaculty"]).ToString()).Append(numberid(x).ToString());
                    account.account_id = id.ToString();
                    account.role_id = "ST";
                    account.falcuty_id = Session["accountFaculty"].ToString();
                }
                else if (account.role_id == "MC")
               {
                    id.Append("MC").Append((Session["accountFaculty"]).ToString()).Append(numberid(x).ToString());
                    account.account_id = id.ToString();
                    account.role_id = "MC";
                    account.falcuty_id = Session["accountFaculty"].ToString();
                }
                else
                {
                    id.Append(account.role_id).Append(numberid(x).ToString());
                    account.account_id = id.ToString();
                }
                account.active = "Activated";
                account.account_date = DateTime.Now;
                db.accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.falcuty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", account.falcuty_id);
            ViewBag.role_id = new SelectList(db.roles, "role_id", "role_name", account.role_id);
            return View(account);
        }
        public RedirectToRouteResult Lock(string id)
        {
         
            account account = db.accounts.Find(id);
            if (ModelState.IsValid)
            {
                account.active = "Locked";
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexActivated");
            }
            ViewBag.falcuty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", account.falcuty_id);
            ViewBag.role_id = new SelectList(db.roles, "role_id", "role_name", account.role_id);
            return RedirectToAction("IndexActivated");

        }
        public RedirectToRouteResult Activate(string id)
        {

            account account = db.accounts.Find(id);
            if (ModelState.IsValid)
            {
                account.active = "Activated";
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexLocked");
            }
            ViewBag.falcuty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", account.falcuty_id);
            ViewBag.role_id = new SelectList(db.roles, "role_id", "role_name", account.role_id);
            return RedirectToAction("IndexLocked");

        }
        
        public ActionResult Edit(string id)
        {



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.falcuty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", account.falcuty_id);
            ViewBag.role_id = new SelectList(db.roles, "role_id", "role_name", account.role_id);
            return View(account);
        }

        // POST: accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,account_name,password,Dob,phone,email,address,sex,account_date,role_id,avatar,active,falcuty_id")] account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.falcuty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", account.falcuty_id);
            ViewBag.role_id = new SelectList(db.roles, "role_id", "role_name", account.role_id);
            return View(account);
        }

        // GET: accounts/Delete/5
        public ActionResult Delete(string id)
        {
            var model = db.accounts.SingleOrDefault(b => b.account_id.Equals(id));
            if (model != null)
            {
                db.accounts.Remove(model);
                db.SaveChanges();
                return RedirectToAction("Index", "accounts");
            }
            return View();
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
