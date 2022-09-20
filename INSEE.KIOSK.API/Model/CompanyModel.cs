using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    public class CompanyModel 
    {
        [Display]
        public virtual int Code { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Country Code is required")]
        [MaxLength(2),MinLength(2)]
        [RegularExpression("LK|TI", ErrorMessage = "Enter LK or TI only")]
        public string Country_Code { get; set; }

        [Display]
        public string Last_Modified_By { get; set; }
        [Display]
        public string Last_Modified_Date { get; set; }
        [Display]
        public virtual bool IsActive { get; set; }
    }

    public class UpdateCompanyModel : CompanyModel
    {
        
        [Required(ErrorMessage ="Company Code is required")]
        public override int Code { get; set; }

        [Required]
        public override bool IsActive { get; set; } 
    }




}
//https://stackoverflow.com/questions/36007939/validation-of-liststring-via-regex-in-model-annotation