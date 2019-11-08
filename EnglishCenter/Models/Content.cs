namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Content()
        {
            Comments = new HashSet<Comment>();
        }

        [Key]
        public long content_id { get; set; }

        public string content_title { get; set; }

        public string content_post { get; set; }

        public string status { get; set; }

        public DateTime? date_post { get; set; }

        [StringLength(50)]
        public string PeopleID { get; set; }

        [StringLength(50)]
        public string TopicPostID { get; set; }

        public string RepresentImage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Person Person { get; set; }

        public virtual TopicPost TopicPost { get; set; }
    }
}
