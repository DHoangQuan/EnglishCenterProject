namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Class")]
    public partial class Class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Class()
        {
            UsingRooms = new HashSet<UsingRoom>();
        }

        [StringLength(50)]
        public string ClassID { get; set; }

        [StringLength(50)]
        public string LessonID { get; set; }

        [StringLength(50)]
        public string PeopleID { get; set; }

        public int? QuantityMAX { get; set; }

        public int? QuantityMIN { get; set; }

        public string ClassName { get; set; }

        public virtual Lesson Lesson { get; set; }

        public virtual Person Person { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsingRoom> UsingRooms { get; set; }
    }
}
