using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MVCBlog.Controllers
{
    public class HomeController : Controller

    {
        private ApplicationDbContext db = new
            ApplicationDbContext();
        //Get Recipes
        public ActionResult Index(string Searchby,string search)
        {
            if(Searchby=="Name")
            {
                return View(db.Recipes.Where(x => x.Title == search).ToList());
            }
            else if (Searchby == "Ingredients")
            {
                return View(db.Recipes.Where(x => x.Body.Contains(search)).ToList());
            }
            var recipes = db.Recipes.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(3);
            return View(recipes.ToList());
        }
    }
      
    
}
