namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slot")]
    public partial class Slot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Slot()
        {
            UsingRooms = new HashSet<UsingRoom>();
        }

        [StringLength(50)]
        public string SlotID { get; set; }

        public string SlotName { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsingRoom> UsingRooms { get; set; }
    }
}
