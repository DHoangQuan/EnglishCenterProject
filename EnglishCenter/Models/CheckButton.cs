using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class CheckButton
    {
        public int UsingRoomID { get; set; }

        [StringLength(50)]
        public string RoomID { get; set; }

        [StringLength(50)]
        public string SlotID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTime { get; set; }

        public string DateTimeStart { get; set; }

        

    }
}