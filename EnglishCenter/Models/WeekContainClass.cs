using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class WeekContainClass
    {

        public int UsingRoomID { get; set; }

        [StringLength(50)]
        public string RoomID { get; set; }

        [StringLength(50)]
        public string SlotID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public string DayOfWeek { get; set; }

        [StringLength(50)]
        public string ClassID { get; set; }

        public string UseFor { get; set; }

        public string Notes { get; set; }
        [StringLength(50)]
        public string LessonID { get; set; }
        //public virtual Class Class { get; set; }

        //public virtual Room Room { get; set; }

        //public virtual Slot Slot { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }
        public string ClassName { get; set; }
    }
}