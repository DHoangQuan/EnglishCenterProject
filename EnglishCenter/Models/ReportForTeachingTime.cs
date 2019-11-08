using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class ReportForTeachingTime
    {
        public string PeopleID { get; set; }

        public string Name { get; set; }
        public int Year { get; set; }
        public long TotalTime { get; set; }
        public int TeachingTimesJan { get; set; }
        public int TeachingTimesFeb { get; set; }
        public int TeachingTimesMar { get; set; }
        public int TeachingTimesApr { get; set; }
        public int TeachingTimesMay { get; set; }
        public int TeachingTimesJune { get; set; }
        public int TeachingTimesJuly { get; set; }
        public int TeachingTimesAug { get; set; }
        public int TeachingTimesSep { get; set; }
        public int TeachingTimesOct { get; set; }
        public int TeachingTimesNov { get; set; }
        public int TeachingTimesDec { get; set; }
        public double AverageTimes { get; set; }
    }
}