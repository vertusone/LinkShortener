using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Links.Models
{
    public class Link
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, StringLength(200)]
        public string LongUrl { get; set; }

        [StringLength(15)]
        public string UrlKey { get; set; }

        public DateTimeOffset InsertionDate { get; set; }

        public int RedirectsCount { get; set; }
    }
}