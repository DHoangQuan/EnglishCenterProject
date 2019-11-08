using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EnterPriseWeb.Models;
using System.Text;
using iTextSharp.text.pdf;
using SelectPdf;
using System.IO;
using System.Web;
using Ionic.Zip;

using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;

namespace EnterPriseWeb.Controllers
{
    public class contentsController : Controller
    {
        private ModelContext db = new ModelContext();
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
        // POST: contents/Create
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
        public ActionResult IndexHandle(string process)
        {

            var contents = db.contents.Include(c => c.account).Include(c => c.faculty).Where(p => p.status == process);
            //List();
            return View(contents.ToList());
        }
        
        public ActionResult Guest()
        {

            var contents = db.contents.Include(c => c.account).Include(c => c.faculty).Where(p => p.status == "Public");
            //List();
            return View(contents.ToList());
        }
        public ActionResult IndexFaculty(string id)
        {

            var contents = db.contents.Include(c => c.account).Include(c => c.faculty).Where(p => p.faculty_id == id && p.status == "Public");
            //List();
            Session["FAC"] = id;
            return View(contents.ToList());
        }
        public ActionResult StudentHistory()
        {
            
            var fcsdate = db.closuredates.Where(c => c.Year == DateTime.Now.Year).Select(s => s.Final_Closure_date).FirstOrDefault();
            int result = DateTime.Compare(fcsdate ?? DateTime.Now, DateTime.Now);
            if (result > 0)
            {
                Session["fabc"] = 2;
            }
            else
            {
                Session["fabc"] = 1;
            }
            string session = Session["accountID"].ToString();
            var contents = db.contents.Include(c => c.account).Include(c => c.faculty).Where(p => p.account_id == session);

            return View(contents.ToList());
        }
        public ActionResult Index()
        {
            var csdate = db.closuredates.Where(c => c.Year == DateTime.Now.Year).Select(s => s.Closure_date).FirstOrDefault();
            
            int result = DateTime.Compare(csdate ?? DateTime.Now, DateTime.Now);
            if(result > 0)

            {
                Session["abc"] = 1;
            }else if(result < 0)
            {
                Session["abc"] = 2;
            } else
            {
                Session["fabc"] = 3;
            }
            var contents = db.contents.Include(c => c.account).Include(c => c.faculty).Where(p => p.status == "Public");

            return View(contents.ToList());
        }
        public ActionResult IndexFASt(string id)
        {
            var contents = db.contents.Include(c => c.account).Include(c => c.faculty).Where(p => p.faculty_id == id && p.status == "Public");
            //List();
            Session["FAC"] = id;

            return View(contents.ToList());
        }
        public ActionResult IndexGenerallAdmin()
        {
            var fcsdate = db.closuredates.Where(c => c.Year == DateTime.Now.Year).Select(s => s.Final_Closure_date).FirstOrDefault();
            int result = DateTime.Compare(fcsdate ?? DateTime.Now, DateTime.Now);
            if (result > 0)
            {
                Session["fabc"] = 1;
            }
            else if (result < 0)
            {
                Session["fabc"] = 2;
            }else
            {
                Session["fabc"] = 3;
            }
            var contents = db.contents.Include(c => c.account).Include(c => c.faculty);
            
            return View(contents.ToList());
        }
        public ActionResult DetailsinGenerall(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            content content = db.contents.Find(id);
            //DateTime UpdatedTime = content.date_post ?? DateTime.Now;
            //List(UpdatedTime);

            if (content == null)
            {
                return HttpNotFound();
            }
            return PartialView("../contents/DetailsinGenerall", content);
        }
        public ActionResult ListComment(string id)
        {

            var comment = db.comments.Where(e => e.content_id == id);


            return View(comment.ToList());
        }

        // GET: Reports
        //public ActionResult IndexReport(string year)
        //{

        //    //int y = Convert.ToInt32(year);
        //    long intYear = Convert.ToInt64(year);
        //    var faculty = db.faculties;
        //    var stCont = db.contents.Where(c => c.date_post.Value.Year == intYear);
        //    List<DataPoint> datapoint = new List<DataPoint>();
        //    int total = stCont.Count();
        //    foreach (var idfa in faculty)
        //    {
        //        var contCount = 0;
        //        ArrayList accList = new ArrayList();
        //        foreach (var contfa in stCont.Where(s => s.faculty.faculty_id == idfa.faculty_id))
        //        {
        //            contCount++;
        //            int i;
        //            for (i = 0; i < accList.Count && !accList[i].Equals(contfa.account_id); i++) ;

        //            if (i == accList.Count)
        //            {
        //                accList.Add(contfa.account_id);
        //            }
        //        }
        //        float percent = (float)contCount / (float)total;
        //        datapoint.Add(new DataPoint()
        //        {
        //            faculty_id = idfa.faculty_id,
        //            faculty_name = idfa.faculty_name,
        //            Numberofcontribution = contCount,
        //            years = year,
        //            PercentOfIssue = String.Format("{0:P2}", percent),
        //            NumberOfAccount = accList.Count,
        //        });
        //    }
        //    ViewBag.YearReport = year;
        //    Session["year"] = year;
        //    return View(datapoint);
        //}
        public ActionResult IndexReport(string year)
        {

            //int y = Convert.ToInt32(year);
            long intYear = Convert.ToInt64(year);
            var faculty = db.faculties;
            var stCont = db.contents.Where(c => c.date_post.Value.Year == intYear);
            var contpub = db.contents.Where(c => c.date_public.Value.Year == intYear);
            List<DataPoint> datapoint = new List<DataPoint>();
            int total = stCont.Count();
            foreach (var idfa in faculty)
            {
                var contCount = 0;
                var publiccontent = 0;
                foreach (var contfa1 in contpub.Where(s => s.faculty.faculty_id == idfa.faculty_id))
                {
                    publiccontent++;
                }
                ArrayList accList = new ArrayList();
                foreach (var contfa in stCont.Where(s => s.faculty.faculty_id == idfa.faculty_id))
                {
                    contCount++;
                    int i;
                    for (i = 0; i < accList.Count && !accList[i].Equals(contfa.account_id); i++) ;

                    if (i == accList.Count)
                    {
                        accList.Add(contfa.account_id);
                    } 
                }
                float percent = (float)contCount / (float)total;
                datapoint.Add(new DataPoint()
                {
                    faculty_id = idfa.faculty_id,
                    faculty_name = idfa.faculty_name,
                    Numberofcontribution = contCount,
                    PercentOfIssue = String.Format("{0:P2}", percent),
                    NumberOfAccount = accList.Count,
                    Numberofcontributionpublic = publiccontent,
                });
            }
            ViewBag.YearReport = year;
            Session["year"] = year;
            return View(datapoint);
        }


        public ActionResult exportReport()
        {

            long intYear = Convert.ToInt64(Session["year"].ToString());
            var faculty = db.faculties;
            var stCont = db.contents.Where(c => c.date_post.Value.Year == intYear);
            var contpub = db.contents.Where(c => c.date_public.Value.Year == intYear);
            List<DataPoint> datapoint = new List<DataPoint>();
            int total = stCont.Count();
            foreach (var idfa in faculty)
            {
                var contCount = 0;
                var publiccontent = 0;
                foreach (var contfa1 in contpub.Where(s => s.faculty.faculty_id == idfa.faculty_id))
                {
                    publiccontent++;
                }
                ArrayList accList = new ArrayList();
                foreach (var contfa in stCont.Where(s => s.faculty.faculty_id == idfa.faculty_id))
                {
                    contCount++;
                    int i;
                    for (i = 0; i < accList.Count && !accList[i].Equals(contfa.account_id); i++) ;

                    if (i == accList.Count)
                    {
                        accList.Add(contfa.account_id);
                    }
                }
                float percent = (float)contCount / (float)total;
                datapoint.Add(new DataPoint()
                {
                    faculty_id = idfa.faculty_id,
                    faculty_name = idfa.faculty_name,
                    Numberofcontribution = contCount,
                    PercentOfIssue = String.Format("{0:P2}", percent),
                    NumberOfAccount = accList.Count,
                    Numberofcontributionpublic = publiccontent,
                    years = Session["year"].ToString(),
                });
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport.rpt"));
            rd.SetDataSource(datapoint.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Report.pdf");

            }
            catch
            {
                throw;
            }
           
            //return View();
        }

        public ActionResult Createcmt()
        {

            return PartialView("../contents/_partialCreate");
        }

        // POST: comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createcmt(comment comment, string id)
        {
            if (ModelState.IsValid)
            {
                comment.content_id = id;

                comment.account_id = Session["accountID"].ToString();
                comment.comment_date = DateTime.Now;
                db.comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "contents", new { id = id });
            }

            
            return RedirectToAction("Index", "accounts");
        }


        // GET: contents/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            content content = db.contents.Find(id);
            //DateTime UpdatedTime = content.date_post ?? DateTime.Now;
            //List(UpdatedTime);

            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }
        // GET: contents/Create
        public ActionResult Create()
        {
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "account_name");
            ViewBag.faculty_id = new SelectList(db.faculties, "faculty_id", "faculty_name");
            return View();
        }

        public RedirectToRouteResult PublicPost(string id)
        {

            content content = db.contents.Find(id);
            if (ModelState.IsValid)
            {
                content.status = "Public";
                content.date_post = DateTime.Now;   
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexGenerallAdmin");
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "account_name", content.account_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", content.faculty_id);
            return RedirectToAction("IndexGenerallAdmin");

        }
        public RedirectToRouteResult EnablePost(string id)
        {

            content content = db.contents.Find(id);
            if (ModelState.IsValid)
            {
                content.status = "Enable";
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexGenerallAdmin");
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "account_name", content.account_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", content.faculty_id);
            return RedirectToAction("IndexGenerallAdmin");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "content_id,content_title,content_post,account_id,status,faculty_id,date_post,date_public,date_end")] content content)
        {
            int m = DateTime.Now.Year;
            var closedate = db.closuredates.Where(c => c.Year == m).Select(md => md.Closure_date).FirstOrDefault();
            var finalclosedate = db.closuredates.Where(c => c.Year == m).Select(md => md.Final_Closure_date).FirstOrDefault();

            StringBuilder sb = new StringBuilder();
            var x = RandomNumber(1, 99999);
            
            if (ModelState.IsValid)
            {

                sb.Append(content.faculty_id).Append(numberid(x));
                content.content_id = sb.ToString();

                content.date_post = DateTime.Now;
                content.date_end = closedate;
                content.date_final_end = finalclosedate;
                //content.date_end = DateTime.Now.AddDays(14);
                content.faculty_id = Session["accountFaculty"].ToString();
                content.account_id = Session["accountID"].ToString();
                content.status = "Processing";
                db.contents.Add(content);
                db.SaveChanges();

                SendEmail(sb.ToString(), Session["accountID"].ToString(), content.content_title);


                return RedirectToAction("Index");
            }


            
            return View(content);
        }

        public ActionResult SendEmail(string id, string acid, string tit)
        {

            var x = Session["accountFaculty"].ToString();

            var acc = db.accounts.Where(f => f.falcuty_id == x && f.role_id == "MC").ToList();

            ArrayList emailArray = new ArrayList();
            foreach (var item in acc.Select(s => s.email))
            {
                emailArray.Add(item);
            }
            foreach (string email in emailArray)
            {
                //StringBuilder sb = new StringBuilder();
                //send new confirmation email
                const string username = "TCS2001Send@gmail.com";
                const string password = "123ASDzxc";
                SmtpClient smtpclient = new SmtpClient();
                MailMessage mail = new MailMessage();
                MailAddress fromaddress = new MailAddress("TCS2001Send@gmail.com");
                smtpclient.Host = "smtp.gmail.com";
                smtpclient.Port = 587;
                mail.From = fromaddress;
                mail.To.Add(email);
                mail.Subject = "New Upload from " + acid.ToString();
                mail.IsBodyHtml = true;
                var name = db.accounts.SingleOrDefault(n => n.email == email);

                mail.Body = "<p>Dear Mr/Mrs " + name.account_name
                    + ",<br />"
                    + "you have new upload is waiting to handle <br />"
                    + "The title: " + tit
                    + "<br />"
                    + "ID of article: "
                    + id
                    + "<br />This mail is from the system, please not reply back."
                    ;


                smtpclient.EnableSsl = true;
                smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpclient.Credentials = new NetworkCredential(username, password);
                smtpclient.Send(mail);
            }

            //return RedirectToAction("Index", "contents");
            return View();

        }
        // GET: contents/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            content content = db.contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "account_name", content.account_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", content.faculty_id);
            return View(content);
        }

        // POST: contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "content_id,content_title,content_post,account_id,status,faculty_id,date_post,date_public,date_end")] content content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "account_name", content.account_id);
            ViewBag.faculty_id = new SelectList(db.faculties, "faculty_id", "faculty_name", content.faculty_id);
            return View(content);
        }

        // GET: contents/Delete/5
        public ActionResult Delete(string id)
        {
            var model = db.contents.SingleOrDefault(b => b.content_id.Equals(id));
            if (model != null)
            {
                db.contents.Remove(model);
                db.SaveChanges();
                return RedirectToAction("Indexhandle", "contents", new { process = "Enable" });
            }
            return View();
        }

        // POST: contents/Delete/5
        

        // select pdf library
        public FileStreamResult DownloadZip(string id)
        {
            MemoryStream workStream = new MemoryStream();
            content content = db.contents.Find(id);
            // read parameters from the webpage
            string htmlString = content.content_post;


            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = 1024;
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                webPageHeight = 0;
            }
            catch { }

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString);

            byte[] pdf = doc.Save();
            doc.Close();
            // put pdf into zip
            using (var zip = new ZipFile())
            {

                zip.AddEntry(content.content_id + ".pdf", pdf);
                zip.Save(workStream);
            }
            workStream.Position = 0;
            FileStreamResult fileResult = new FileStreamResult(workStream, System.Net.Mime.MediaTypeNames.Application.Zip);
            fileResult.FileDownloadName = content.content_id + ".zip";
            return fileResult;
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
