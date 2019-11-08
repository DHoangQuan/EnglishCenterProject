using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class WeekContainClassAndSlot
    {
        public int NumberofClassUsingRoom { get; set; }

        [StringLength(50)]
        public string SlotID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public int numberofreadingskill { get; set; }
        public int numberofwritingskill { get; set; }
        public int numeberoflisteningskill { get; set; }
        public int numberofspeakingskill { get; set; }
    }
}