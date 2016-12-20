using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        //Get: User/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var users = database.Users
                    .ToList();

                var admins = GetAdminUsernames(users, database);
                ViewBag.Admins = admins;

                return View(users);
            }
        }
        private HashSet<string>GetAdminUsernames(List<ApplicationUser>users,ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();
            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id,"Admin"))
                {
                    admins.Add(user.Email);
                }
            }
            return admins;
        }

        //
        // GET: User/Edit
        public ActionResult Edit(string id)
        {
            //Validate Id
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                //Get user from database

                var user = database.Users
                    .Where(u => u.Id == id)
                    .First();

                //Check if user exists

                if (user==null)
                {
                    return HttpNotFound();
                }

                //Create a view model
                var viewModel = new EditUserViewModel();
                viewModel.User = user;
                viewModel.Roles = GetUserRoles(user, database);

                //Pass the model to the view
                return View(viewModel);
            }
        }
            private List<Role>GetUserRoles(ApplicationUser user, ApplicationDbContext db)
        {
            //Create User manager
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();
            //Get all application roles
            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();
            //For each application role, check if user has it
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.isSelected = true;
                }

                userRoles.Add(role);
            }
            //Return a list with all roles
            return userRoles;

        }

        //
        //POST: User/Edit
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    //Get user from database
                    var user = database.Users.FirstOrDefault(u => u.Id == id);

                    //Check if user exists
                    if (user==null)
                    {
                        return HttpNotFound();
                    }

                    // If password field is not empty , change password
                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        this.ChangeUserPassword(id, viewModel);
                    }

                    //set user roles
                    this.SetUserRoles(viewModel, user, database);

                    //save changes
                    database.Entry(user).State = EntityState.Modified;
                    database.SaveChanges();
                    return RedirectToAction("List");

                }

            }
            return View(viewModel);
           
        }
        private void ChangeUserPassword(string userId, EditUserViewModel viewModel)
        {
            //Create user manager

            var userManager = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            //Create password reset token
            var token = userManager.GeneratePasswordResetToken(userId);
            var result = userManager.ResetPassword(userId, token, viewModel.Password);
            

            //Check if operation succeeded
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
        private void SetUserRoles(EditUserViewModel viewModel,ApplicationUser user,ApplicationDbContext context)
        {
            var userManager = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in viewModel.Roles)
            {
                if (role.isSelected && !userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.isSelected && userManager.IsInRole(user.Id,role.Name))
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }

            }
        }

        //
        //GET: User/Delete
        public ActionResult Delete(string id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                //Get user from database
                var user = database.Users
                    .Where(u => u.Id.Equals(id))
                    .First();
                //Check if user exists
                if (user==null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
        }

        //
        // POST: User/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                //Get user from database
                var user = database.Users
                    .Where(u => u.Id.Equals(id))
                    .First();
                //Get user recipes from database
                var userRecipes = database.Recipes
                    .Where(r => r.Author.Id == user.Id);
                //Delete user recipes
                foreach (var recipe in userRecipes)
                {
                    database.Recipes.Remove(recipe);
                }
                //Delete user and save changes
                database.Users.Remove(user);
                database.SaveChanges();
                return RedirectToAction("List");

            }
        }
    }
}