namespace EnglishCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quote")]
    public partial class Quote
    {
        [StringLength(50)]
        public string QuoteID { get; set; }

        public string Titlle { get; set; }

        public string Author { get; set; }

        public string ContentQuote { get; set; }

        public string Status { get; set; }
    }
}
