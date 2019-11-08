namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LecturerTime")]
    public partial class LecturerTime
    {
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string LecturerID { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        public long? TeachingTime { get; set; }

        public virtual Person Person { get; set; }
    }
}
