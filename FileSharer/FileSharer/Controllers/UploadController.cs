using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingApp.Controllers
{
    public class UploadController : Controller
    {
        MyDbContext _context;
        IHostingEnvironment _appEnvironment;

        public UploadController(MyDbContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.FileClasses.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileClass file = new FileClass { ShortDesc = uploadedFile.FileName, Path = path };
                _context.FileClasses.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult WithPassword()
        {
            return View();
        }

        public IActionResult WithCounter()
        {
            return View();
        }

        public IActionResult Download (string url)
        {
            return View();
        }
    }
}