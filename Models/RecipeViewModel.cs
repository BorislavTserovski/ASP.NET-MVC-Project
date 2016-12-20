using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBlog.Models
{
    public class RecipeViewModel
    {

       public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }


        [DisplayName("Recipe")]
        public string Body { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        public byte[] Image { get; set; }


        [DisplayName("Choose a Category")]
        [Required]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

    }
}