//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCBlog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recipes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public System.DateTime Date { get; set; }
        public string Author_Id { get; set; }
    
        public virtual Cocktails AspNetUsers { get; set; }
    }
}