using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam2.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        readonly ApplicationDbContext context;

        public AccountController(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ChangeRole()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (roleManager.Roles.Any(x => x.Name.ToLower() == "admin"))
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            if (roleManager.Roles.Any(x => x.Name.ToLower() == "user"))
                await roleManager.CreateAsync(new IdentityRole { Name = "user" });
            if (User.IsInRole("admin"))
            {
                await userManager.RemoveFromRoleAsync(user, "admin");
                await userManager.AddToRoleAsync(user, "user");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, "user");
                await userManager.AddToRoleAsync(user, "admin");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}