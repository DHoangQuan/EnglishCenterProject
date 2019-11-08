namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentTime")]
    public partial class StudentTime
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(50)]
        public string StudentID { get; set; }

        public long? Studied { get; set; }

        public long? Future { get; set; }

        public virtual Person Person { get; set; }
    }
}
