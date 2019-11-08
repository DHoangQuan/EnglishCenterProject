namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsingRoom")]
    public partial class UsingRoom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsingRoom()
        {
            QuantityAndAttendances = new HashSet<QuantityAndAttendance>();
        }

        public int UsingRoomID { get; set; }

        [StringLength(50)]
        public string RoomID { get; set; }

        [StringLength(50)]
        public string SlotID { get; set; }

        public DateTime? Date { get; set; }

        public string DayOfWeek { get; set; }

        [StringLength(50)]
        public string ClassID { get; set; }

        public int? QuatityRegister { get; set; }

        public string UseFor { get; set; }

        public string Status { get; set; }

        public string Notes { get; set; }

        public virtual Class Class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuantityAndAttendance> QuantityAndAttendances { get; set; }

        public virtual Room Room { get; set; }

        public virtual Slot Slot { get; set; }
    }
}
