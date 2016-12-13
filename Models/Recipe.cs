using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBlog.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Name")]
        public string Title { get; set; }
        [DisplayName("Recipe")]
        [Required]
        public string Body { get; set; }

        [Required]
        [DisplayName("Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        
        public ApplicationUser Author { get; set;}
    }
}