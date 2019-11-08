using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class ReportForCustome7DaysLessons
    {
        public string LessonID { get; set; }
        public string LessonName { get; set; }
        public int repeattimesoflesson { get; set; }
        public string percentin7Days { get; set; }
        public string TopicID { get; set; }
        public string TopicName { get; set; }
    }
}