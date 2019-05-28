using System;
using System.Collections.Generic;
using System.IO;
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

        public IActionResult Details(string shortUrl)
        {
            var a = _context.FileClasses.FirstOrDefault(x => x.ShortUrl == shortUrl);

            if (a == null)
            {
                return NotFound();
            }

            ViewData["shorturl"] = Request.Scheme + "://" + Request.Host + "/" + a.ShortUrl; ;
            return View(a);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string key)
        {
            var file = _context.FileClasses.FirstOrDefault(x => x.ShortUrl == key);
            if (file != null)
            {
                var path = _appEnvironment.WebRootPath + file.Path;
                file.DownloadCount++;
                await _context.SaveChangesAsync();
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, file.Type);
            }
            return NotFound();
        }
    }
}