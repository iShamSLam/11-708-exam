using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Models
{
    public class FileClass
    {
        public int ID { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string ShortUrl { get; set; }
        public string Password { get; set; }
        public int DownloadCount { get; set; }
        public int MaxDownloads { get; set; }
        public string Path { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime MaxDate { get; set; }
        public FileParameter Parameter { get; set; }
        public string Type { get; set; }

        public enum FileParameter
        {
            simple,
            password,
            count,
            data
        }
    }
}
