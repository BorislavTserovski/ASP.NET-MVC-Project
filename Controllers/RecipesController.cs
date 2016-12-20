using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace MVCBlog.Controllers
{
    public class RecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipes
        public ActionResult Index()
        {
            return View(db.Recipes.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // GET: Recipes/Create
        [Authorize]
        public ActionResult Create()
        {
            using (var database = new ApplicationDbContext())
            {
                
                var model = new RecipeViewModel();
                model.Categories = database.Categories
                    .OrderBy(c=>c.Name)
                    .ToList();

                return View(model);
            }

        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Body,Date,Id,CategoryId")] RecipeViewModel model, HttpPostedFileBase file)
        {
            using (var database = new ApplicationDbContext())
            {
                var recipe = new Recipe();
                var authorId = database.Users
                    .Where(u => u.UserName == this.User.Identity.Name)
                    .First()
                    .Id;
                
                recipe.AuthorId = authorId;
                recipe.Body = model.Body;
                recipe.Title = model.Title;
                recipe.CategoryId = model.CategoryId;

                if (ModelState.IsValid)
                {
                    using (MemoryStream ms = new MemoryStream())
                        if (file != null)
                        {


                            file.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                            recipe.Image = array;

                        }

                    recipe.Date = DateTime.Now;

                    db.Recipes.Add(recipe);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(recipe);
            }

           

            
        }

        // GET: Recipes/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new ApplicationDbContext())
            {
                var recipe = database.Recipes
                    .Where(r => r.Id == id)
                    .First();
                var model = new RecipeViewModel();
                model.Id = recipe.Id;
                model.Title = recipe.Title;
                model.Body = recipe.Body;
                model.Image = recipe.Image;
                model.CategoryId = recipe.CategoryId;
                model.Categories = database.Categories
                    .OrderBy(c => c.Name)
                    .ToList();
                if (!isUserAuthorizedToEdit(recipe))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                if (recipe == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date,CategoryId")] RecipeViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var recipe = database.Recipes
                        .FirstOrDefault(r => r.Id == model.Id);

                    recipe.Title = model.Title;
                    recipe.Body = model.Body;
                    recipe.CategoryId = model.CategoryId;
                    if (file != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                            recipe.Image = array;
                        }
                    }

                    recipe.Date = DateTime.Now;
                    database.Entry(recipe).State = EntityState.Modified;
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Recipes/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (!isUserAuthorizedToEdit(recipe))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            db.Recipes.Remove(recipe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private bool isUserAuthorizedToEdit(Recipe recipe)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            //bool isAuthor = recipe.isAuthor(this.User.Identity.GetUserId());
            return isAdmin;//|| isAuthor;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
