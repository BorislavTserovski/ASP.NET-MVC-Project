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
        public Recipe()
        {
        }
        public Recipe(string authorId, string title, string body,int categoryId)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Body = body;
            this.CategoryId = categoryId;
        }

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

       
        
        public ApplicationUser Author { get; set;}

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

      
    }
}