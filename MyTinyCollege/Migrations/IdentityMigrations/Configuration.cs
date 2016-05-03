namespace MyTinyCollege.Migrations.IdentityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTinyCollege.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\IdentityMigrations";
            ContextKey = "MyTinyCollege.Models.ApplicationDbContext";
        }

        protected override void Seed(MyTinyCollege.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            // seeding identity with role and admin users
            //add admin roll
            if(!(context.Roles.Any(r=>r.Name == "admin")))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInsert = new IdentityRole { Name = "admin" };
                roleManager.Create(roleToInsert);
            }
            //add student
            if (!(context.Roles.Any(r => r.Name == "student")))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInsert = new IdentityRole { Name = "student" };
                roleManager.Create(roleToInsert);
            }
            //add instructor
            if (!(context.Roles.Any(r => r.Name == "instructor")))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInsert = new IdentityRole { Name = "instructor" };
                roleManager.Create(roleToInsert);
            }
            //add admin user assign admin role
            if (!(context.Users.Any(r => r.UserName == "admin@tinycollege.com")))
            {
                var userStore = new UserStore<Models.ApplicationUser>(context);
                var userManager = new UserManager<Models.ApplicationUser>(userStore);
                var userToInsert = new Models.ApplicationUser { UserName= "admin@tinycollege.com", Email="admin@tinycollege.com", EmailConfirmed=true };
                userManager.Create(userToInsert, "Administrator");
                userManager.AddToRole(userToInsert.Id, "admin");
                }
        }
    }
}
