using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static FileSharingApp.Models.FileClass;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, String shortDescription, String longDescription)
        {
            if (uploadedFile != null)
            {
                string sh = GetUniqueKey(6);
                var b = CheckKey(sh);
                while (b)
                {
                    sh = GetUniqueKey(6);
                    b = CheckKey(sh);
                }
                string path = "/Files/" + uploadedFile.FileName;
                string type = uploadedFile.ContentType;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileClass file = new FileClass { ShortDesc = shortDescription, LongDesc = longDescription, Type = type, Parameter = FileParameter.simple,Path = path, UploadDate = DateTime.Now, ShortUrl = sh };
                _context.FileClasses.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Main");
        }

        private string GetUniqueKey(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private bool CheckKey(string key)
        {
            var a = _context.FileClasses.FirstOrDefault(x => x.ShortUrl == key);
            if (a != null && a.ShortUrl == key)
            {
                switch (a.Parameter)
                {
                    case FileParameter.simple:
                        return false;

                    case FileParameter.password:
                        return false;

                    case FileParameter.count:
                        if (a.MaxDownloads == 0)
                        {
                            _context.FileClasses.Remove(a);
                            _context.SaveChanges();
                            return true;
                        }
                        return false;

                    case FileParameter.data:
                        if (a.MaxDate < DateTime.Now)
                        {
                            _context.FileClasses.Remove(a);
                            _context.SaveChanges();
                            return true;
                        }
                        return false;
                }
            }
            return false;
        }

        public IActionResult WithPassword()
        {
            return View();
        }

        public IActionResult WithCounter()
        {
            return View();
        }
    }
}