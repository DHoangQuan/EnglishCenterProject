using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class LessonHasNoBeTaught
    {
        [StringLength(50)]
        public string LessonID { get; set; }

        public string LessonName { get; set; }

        [StringLength(50)]
        public string TopicID { get; set; }

        [StringLength(50)]
        public string Topicname { get; set; }

        public string Resource { get; set; }

        public int? RepeatTimes { get; set; }
        public virtual Topic Topic { get; set; }
    }
}