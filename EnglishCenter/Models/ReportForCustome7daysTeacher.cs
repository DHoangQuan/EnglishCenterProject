using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class ReportForCustome7daysTeacher
    {
        public string LecturerID { get; set; }
        public string Name { get; set; }
        public int teachingslotin7days { get; set; }
        public int teachingreading { get; set; }
        public int teachingwriting { get; set; }
        public int teachinglistening { get; set; }
        public int teachingspeaking { get; set; }
        public string percentofteachingin7days { get; set; }
    }
}