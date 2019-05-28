using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam2.Models;
using Exam2.Data;
using Exam2.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Exam2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext context,
            //RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            //await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            //await _roleManager.CreateAsync(new IdentityRole { Name = "user" });

            //var user = new IdentityUser { Email = "qwe@gmail.com", UserName = "qwe" };
            //var result = await _userManager.CreateAsync(user, "1q2w3e4R!");
            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(user, "admin");
            //}

            var restaurant = new Restaurant() { Name = "big" };
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return View(_context.Restaurants);
        }

        [HttpGet]
        [Route("~/GetDishesFromRestaurant/{restaurantId:int}/")]
        //[Authorize]
        public async Task<IActionResult> GetDishesFromRestaurant(int? restaurantId)
        {
            if (restaurantId == null)
            {
                return NotFound();
            }

            var dishes = _context.Dishes.Where(x => x.RestaurantId == restaurantId);
            var restaurant = _context.Restaurants.Where(x => x.Id == restaurantId).FirstOrDefault();
            ViewData["RestaurantName"] = restaurant.Name;
            ViewData["RestaurantId"] = restaurant.Id;
            return View(dishes);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
