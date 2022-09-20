using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    public class VideoUpload
    {
        public IFormFile Files { get; set; } 
    }
}
