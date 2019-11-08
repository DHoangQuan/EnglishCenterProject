namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        [Key]
        public long comment_id { get; set; }

        public long content_id { get; set; }

        public string comment_content { get; set; }

        public DateTime? comment_date { get; set; }

        [StringLength(50)]
        public string PeopleID { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public virtual Content Content { get; set; }

        public virtual Person Person { get; set; }
    }
}
