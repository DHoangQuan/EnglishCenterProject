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
    public class ReportForTeachingTimeSkillController : Controller
    {
        private ModelContext1 db = new ModelContext1();
        // GET: ReportForTeachingTimeSkill
        public ActionResult ReportOfTeachingSkill(string year, string peopleid)
        {
            Session["ParameterForChartTeachingSkillYear"] = null;
            Session["ParameterForChartTeachingSkillPID"] = null;
            Session["ParameterForChartTeachingSkillYear"] = year;
            Session["ParameterForChartTeachingSkillPID"] = peopleid;
            if (Session["ReportForTeachingTimeSkill"] != null)
            {
                List<ReportForTeachingTimeSkill> lhnbt = Session["ReportForTeachingTimeSkill"] as List<ReportForTeachingTimeSkill>;
                lhnbt.Clear();
            }
            Session["ReportForTeachingTimeSkill"] = new List<ReportForTeachingTimeSkill>();
            List<ReportForTeachingTimeSkill> rpftts = Session["ReportForTeachingTimeSkill"] as List<ReportForTeachingTimeSkill>;
            var GetAllClass = db.UsingRooms.Where(d => d.Class.PeopleID == peopleid && d.Date.Value.Year.ToString() == year);
            var listskills = db.Skills;
            foreach (var skill in listskills)
            {
                int counteachskill = 0;
                foreach (var teachingskill in GetAllClass.Where(c => c.Class.Lesson.Topic.Skill.SkilID == skill.SkilID))
                {
                    counteachskill++;
                }
                float percent = (float)counteachskill / (float)GetAllClass.Count();

                ReportForTeachingTimeSkill newobj = new ReportForTeachingTimeSkill()
                {
                    SKillName = skill.Name,
                    NumberOfTeachingSkill = counteachskill,
                    Percent = String.Format("{0:P2}", percent),
                };
                rpftts.Add(newobj);
                
            }
            //ReportOfTeachingSkill1234(year, peopleid);
            return PartialView("../ReportForTeachingTimeSkill/ReportOfTeachingSkill", rpftts);
        }
        public JsonResult ReportOfTeachingSkill1234()
        {
            string year = Session["ParameterForChartTeachingSkillYear"].ToString();
            string peopleid = Session["ParameterForChartTeachingSkillPID"].ToString();
            if (Session["ReportForTeachingTimeSkill"] != null)
            {
                List<ReportForTeachingTimeSkill> lhnbt = Session["ReportForTeachingTimeSkill"] as List<ReportForTeachingTimeSkill>;
                lhnbt.Clear();
            }
            Session["ReportForTeachingTimeSkill"] = new List<ReportForTeachingTimeSkill>();
            List<ReportForTeachingTimeSkill> rpftts = Session["ReportForTeachingTimeSkill"] as List<ReportForTeachingTimeSkill>;
            var GetAllClass = db.UsingRooms.Where(d => d.Class.PeopleID == peopleid && d.Date.Value.Year.ToString() == year);
            var listskills = db.Skills;
            foreach (var skill in listskills)
            {
                int counteachskill = 0;
                foreach (var teachingskill in GetAllClass.Where(c => c.Class.Lesson.Topic.SkillID == skill.SkilID))
                {
                    counteachskill++;
                }
                float percent = (float)counteachskill / (float)GetAllClass.Count();

                ReportForTeachingTimeSkill newobj = new ReportForTeachingTimeSkill()
                {
                    SKillName = skill.Name,
                    NumberOfTeachingSkill = counteachskill,
                    Percent = String.Format("{0:P2}", percent),
                };
                rpftts.Add(newobj);
                
            }
            return Json(new { rpftts }, JsonRequestBehavior.AllowGet);
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