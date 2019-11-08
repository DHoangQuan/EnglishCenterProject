using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class RoomsHaveNotBeUsed
    {
        [StringLength(50)]
        public string RommID { get; set; }

        public string Floor { get; set; }

        public string RoomNo { get; set; }
    }
}