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
    public class ReportForTeachingTimeController : Controller
    {
        private ModelContext1 db = new ModelContext1();
        // GET: ReportForTeachingTime
        public ActionResult ReportOfTeaching(string year, string idlecturer)
        {
            //string year = "2019";

            if (Session["ReportForTeachingTime"] != null)
            {
                List<ReportForTeachingTime> lhnbt = Session["ReportForTeachingTime"] as List<ReportForTeachingTime>;
                lhnbt.Clear();
            }
            Session["TeachingTimes1"] = null;
            Session["TeachingTimes2"] = null;
            Session["TeachingTimes3"] = null;
            Session["TeachingTimes4"] = null;
            Session["TeachingTimes5"] = null;
            Session["TeachingTimes6"] = null;
            Session["TeachingTimes7"] = null;
            Session["TeachingTimes8"] = null;
            Session["TeachingTimes9"] = null;
            Session["TeachingTimes10"] = null;
            Session["TeachingTimes11"] = null;
            Session["TeachingTimes12"] = null;

            //string idlecturer = Session["PersonID"].ToString();
            var getalltime = db.LecturerTimes.Where(c => c.LecturerID == idlecturer && c.Year == year);
            int number = 0;
            for (int i = 1; i <= 12; i++)
            {
                var CountTimesInMonthCheck = getalltime.Where(c => c.Month == i.ToString());
                var CountTimesInMonth = getalltime.Where(c => c.Month == i.ToString()).FirstOrDefault();
                if (CountTimesInMonthCheck.Count() != 0)
                {
                    number = number + Convert.ToInt32(CountTimesInMonth.TeachingTime.ToString());
                }
            }
            Session["Total"] = number;
            for (int i = 1; i <= 12; i++)
            {
                var CountTimesInMonthCheck = getalltime.Where(c => c.Month == i.ToString());
                var CountTimesInMonth = getalltime.Where(c => c.Month == i.ToString()).FirstOrDefault();
                if (i == 1)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes1"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes1"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 2)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes2"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes2"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 3)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes3"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes3"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 4)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes4"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes4"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 5)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes5"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes5"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 6)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes6"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes6"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 7)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes7"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes7"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 8)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes8"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes8"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 9)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes9"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes9"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 10)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes10"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes10"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 11)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes11"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes11"] = CountTimesInMonth.TeachingTime;
                    }
                }
                else if (i == 12)
                {
                    if (CountTimesInMonthCheck.Count() == 0)
                    {
                        Session["TeachingTimes12"] = 0;
                    }
                    else
                    {
                        Session["TeachingTimes12"] = CountTimesInMonth.TeachingTime;
                    }
                }
            }
            string lecturername = Session["DetailsPersonName"].ToString();
            int total = Convert.ToInt32(Session["Total"].ToString());
            double average = total / 12;
            Session["ReportForTeachingTime"] = new List<ReportForTeachingTime>();
            List<ReportForTeachingTime> rpftt = Session["ReportForTeachingTime"] as List<ReportForTeachingTime>;
            ReportForTeachingTime newobj = new ReportForTeachingTime()
            {
                PeopleID = idlecturer,
                Name = lecturername,
                TotalTime = total,
                Year = Convert.ToInt32(year.ToString()),
                TeachingTimesJan = Convert.ToInt32(Session["TeachingTimes1"].ToString()),
                TeachingTimesFeb = Convert.ToInt32(Session["TeachingTimes2"].ToString()),
                TeachingTimesMar = Convert.ToInt32(Session["TeachingTimes3"].ToString()),
                TeachingTimesApr = Convert.ToInt32(Session["TeachingTimes4"].ToString()),
                TeachingTimesMay = Convert.ToInt32(Session["TeachingTimes5"].ToString()),
                TeachingTimesJune = Convert.ToInt32(Session["TeachingTimes6"].ToString()),
                TeachingTimesJuly = Convert.ToInt32(Session["TeachingTimes7"].ToString()),
                TeachingTimesAug = Convert.ToInt32(Session["TeachingTimes8"].ToString()),
                TeachingTimesSep = Convert.ToInt32(Session["TeachingTimes9"].ToString()),
                TeachingTimesOct = Convert.ToInt32(Session["TeachingTimes10"].ToString()),
                TeachingTimesNov = Convert.ToInt32(Session["TeachingTimes11"].ToString()),
                TeachingTimesDec = Convert.ToInt32(Session["TeachingTimes12"].ToString()),
                AverageTimes = Math.Round(average),
            };
            rpftt.Add(newobj);
            //return Json(new { rpftt }, JsonRequestBehavior.AllowGet);
            return PartialView("../ReportForTeachingTime/ReportOfTeaching", rpftt);
            //return View();
        }
        public ActionResult ReportOfTeaching1(string year)
        {
            if (year == "" || year == null)
            {
                return RedirectToAction("ReportOfTeaching1", "ReportForTeachingTime", new { year = DateTime.Now.Year.ToString() });
            }
            else
            {
                Session["ParameterForReportForTeachingTimeSkill"] = null;
                Session["ParameterForReportForTeachingTimeSkill"] = year;
                //string year = "2019";
                if (Session["ReportForTeachingTime"] != null)
                {
                    List<ReportForTeachingTime> lhnbt = Session["ReportForTeachingTime"] as List<ReportForTeachingTime>;
                    lhnbt.Clear();
                }
                Session["TeachingTimes1"] = null;
                Session["TeachingTimes2"] = null;
                Session["TeachingTimes3"] = null;
                Session["TeachingTimes4"] = null;
                Session["TeachingTimes5"] = null;
                Session["TeachingTimes6"] = null;
                Session["TeachingTimes7"] = null;
                Session["TeachingTimes8"] = null;
                Session["TeachingTimes9"] = null;
                Session["TeachingTimes10"] = null;
                Session["TeachingTimes11"] = null;
                Session["TeachingTimes12"] = null;
                Session["Total"] = null;
                string idlecturer = Session["DetailsPersonID"].ToString();
                var getalltime = db.LecturerTimes.Where(c => c.LecturerID == idlecturer && c.Year == year);
                int number = 0;
                for (int i = 1; i <= 12; i++)
                {
                    var CountTimesInMonthCheck = getalltime.Where(c => c.Month == i.ToString());
                    var CountTimesInMonth = getalltime.Where(c => c.Month == i.ToString()).FirstOrDefault();
                    if (CountTimesInMonthCheck.Count() != 0)
                    {
                        number = number + Convert.ToInt32(CountTimesInMonth.TeachingTime.ToString());
                    }
                }
                Session["Total"] = number;
                for (int i = 1; i <= 12; i++)
                {
                    var CountTimesInMonthCheck = getalltime.Where(c => c.Month == i.ToString());
                    var CountTimesInMonth = getalltime.Where(c => c.Month == i.ToString()).FirstOrDefault();
                    if (i == 1)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes1"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes1"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 2)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes2"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes2"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 3)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes3"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes3"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 4)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes4"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes4"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 5)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes5"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes5"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 6)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes6"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes6"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 7)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes7"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes7"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 8)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes8"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes8"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 9)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes9"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes9"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 10)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes10"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes10"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 11)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes11"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes11"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                    else if (i == 12)
                    {
                        if (CountTimesInMonthCheck.Count() == 0)
                        {
                            Session["TeachingTimes12"] = 0;
                        }
                        else
                        {
                            Session["TeachingTimes12"] = CountTimesInMonth.TeachingTime;
                        }
                    }
                }
                string lecturername = Session["DetailsPersonName"].ToString();
                int total = Convert.ToInt32(Session["Total"].ToString());
                double average = total / 12;
                Session["ReportForTeachingTime"] = new List<ReportForTeachingTime>();
                List<ReportForTeachingTime> rpftt = Session["ReportForTeachingTime"] as List<ReportForTeachingTime>;
                ReportForTeachingTime newobj = new ReportForTeachingTime()
                {
                    PeopleID = idlecturer,
                    Name = lecturername,
                    TotalTime = total,
                    Year = Convert.ToInt32(year.ToString()),
                    TeachingTimesJan = Convert.ToInt32(Session["TeachingTimes1"].ToString()),
                    TeachingTimesFeb = Convert.ToInt32(Session["TeachingTimes2"].ToString()),
                    TeachingTimesMar = Convert.ToInt32(Session["TeachingTimes3"].ToString()),
                    TeachingTimesApr = Convert.ToInt32(Session["TeachingTimes4"].ToString()),
                    TeachingTimesMay = Convert.ToInt32(Session["TeachingTimes5"].ToString()),
                    TeachingTimesJune = Convert.ToInt32(Session["TeachingTimes6"].ToString()),
                    TeachingTimesJuly = Convert.ToInt32(Session["TeachingTimes7"].ToString()),
                    TeachingTimesAug = Convert.ToInt32(Session["TeachingTimes8"].ToString()),
                    TeachingTimesSep = Convert.ToInt32(Session["TeachingTimes9"].ToString()),
                    TeachingTimesOct = Convert.ToInt32(Session["TeachingTimes10"].ToString()),
                    TeachingTimesNov = Convert.ToInt32(Session["TeachingTimes11"].ToString()),
                    TeachingTimesDec = Convert.ToInt32(Session["TeachingTimes12"].ToString()),
                    AverageTimes = Math.Round(average),
                };
                rpftt.Add(newobj);

                return View(rpftt);
            }
        }

    }

}