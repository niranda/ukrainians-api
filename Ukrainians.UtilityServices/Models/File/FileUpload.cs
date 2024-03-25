using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukrainians.UtilityServices.Models.File
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
        public string Username { get; set; }
    }
}
