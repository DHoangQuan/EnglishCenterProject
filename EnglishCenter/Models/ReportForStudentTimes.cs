using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class ReportForStudentTimes
    {
        public string PeopleID { get; set; }

        public string Name { get; set; }
        public int Year { get; set; }
        public long TotalTime { get; set; }
        public int StudentTimesJan { get; set; }
        public int StudentTimesFeb { get; set; }
        public int StudentTimesMar { get; set; }
        public int StudentTimesApr { get; set; }
        public int StudentTimesMay { get; set; }
        public int StudentTimesJune { get; set; }
        public int StudentTimesJuly { get; set; }
        public int StudentTimesAug { get; set; }
        public int StudentTimesSep { get; set; }
        public int StudentTimesOct { get; set; }
        public int StudentTimesNov { get; set; }
        public int StudentTimesDec { get; set; }
        public double StudentAverageTimes { get; set; }
    }
}