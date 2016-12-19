using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DataType(DataType.Date)]
        [DisplayName("Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        
        public byte[] Image { get; set; }

       // public bool isAuthor(string id)
        //{
      //      return this.AuthorId.Equals(id);
      //  }
        
        public ApplicationUser Author { get; set;}
    }
}