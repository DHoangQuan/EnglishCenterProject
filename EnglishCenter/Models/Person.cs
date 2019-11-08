namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            Classes = new HashSet<Class>();
            Comments = new HashSet<Comment>();
            Contents = new HashSet<Content>();
            LecturerTimes = new HashSet<LecturerTime>();
            QuantityAndAttendances = new HashSet<QuantityAndAttendance>();
            StudentTimes = new HashSet<StudentTime>();
        }

        [Key]
        [StringLength(50)]
        public string PeopleID { get; set; }

        public string Name { get; set; }

        public long? Phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DoB { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Active { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        [StringLength(50)]
        public string RoleID { get; set; }

        public virtual Authentication Authentication { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Class> Classes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Content> Contents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LecturerTime> LecturerTimes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuantityAndAttendance> QuantityAndAttendances { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentTime> StudentTimes { get; set; }
    }
}
