using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingApp.Controllers
{
    public class MainController : Controller
    {
        MyDbContext _context;
        IHostingEnvironment _appEnvironment;

        public MainController(MyDbContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.FileClasses.ToList());
        }
    }
}