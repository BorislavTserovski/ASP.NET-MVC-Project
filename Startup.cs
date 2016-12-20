using Microsoft.Owin;
using Owin;
using MVCBlog.Migrations;
using MVCBlog.Models;
using System.Data.Entity;


[assembly: OwinStartupAttribute(typeof(MVCBlog.Startup))]
namespace MVCBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
