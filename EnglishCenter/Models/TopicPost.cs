namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TopicPost")]
    public partial class TopicPost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TopicPost()
        {
            Contents = new HashSet<Content>();
        }

        [StringLength(50)]
        public string TopicPostID { get; set; }

        [StringLength(50)]
        public string CatalogueID { get; set; }

        public string TopicName { get; set; }

        public bool? PinToBar { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public virtual Catalogue Catalogue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Content> Contents { get; set; }
    }
}
