namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuantityAndAttendance")]
    public partial class QuantityAndAttendance
    {
        [Key]
        [StringLength(50)]
        public string QnAID { get; set; }

        public int? UsingRoomID { get; set; }

        [StringLength(50)]
        public string PeopleID { get; set; }

        public string Attendent { get; set; }

        public virtual Person Person { get; set; }

        public virtual UsingRoom UsingRoom { get; set; }
    }
}
