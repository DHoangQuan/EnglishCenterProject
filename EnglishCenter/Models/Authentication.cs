namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Authentication")]
    public partial class Authentication
    {
        [Key]
        [StringLength(50)]
        public string PeopleID { get; set; }

        public string EditAuthetication { get; set; }

        public string Comment { get; set; }

        public string CreatePost { get; set; }

        public string EditPost { get; set; }

        public string Class { get; set; }

        public string Room { get; set; }

        public string Topics { get; set; }

        public string Skills { get; set; }

        public string Lessons { get; set; }

        public string Attendant { get; set; }

        public string EditStudentTime { get; set; }

        public string Notes { get; set; }

        public virtual Person Person { get; set; }
    }
}
