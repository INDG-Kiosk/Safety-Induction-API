using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    public class GuestModel
    {       
        public int Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NIC is required")]
        public string NIC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kisok Code is required")]
        public int siteCode { get; set; } = 0;

       // [Required(ErrorMessage = "Profile Image is required")]
        public  string ProfileImage { get; set; } 
        public int ContractorCode { get; set; }
        public string Lang { get; set; }

    }
}
