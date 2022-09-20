using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    public class ContractorModel
    {
        public virtual int Code { get; set; } 
        public virtual string NameEN { get; set; } = string.Empty;
        public virtual string NameSN { get; set; } = string.Empty;
        public virtual string NameTA { get; set; } = string.Empty;
        public virtual string MailingAddress { get; set; } = string.Empty;
        public virtual bool IsActive { get; set; } = false;
    }

    public class InsertContractorModel : ContractorModel
    {

        [Required(ErrorMessage = "Contractor English Name is required")]
        public override string NameEN { get; set; }

        [Required(ErrorMessage = "Contractor Sinhala Name is required")]
        public override string NameSN { get; set; }

        [Required(ErrorMessage = "Contractor Tamil Name is required")]
        public override string NameTA { get; set; }

        [Required(ErrorMessage = "Contractor Mailing Address is required")]
        public override string MailingAddress { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public override bool IsActive { get; set; }
    }

    public class UpdateContractorModel : ContractorModel
    {

        [Required(ErrorMessage = "Contractor Code is required")]
        public override int Code { get; set; }

        [Required(ErrorMessage = "Contractor English Name is required")]
        public override string NameEN { get; set; }

        [Required(ErrorMessage = "Contractor Sinhala Name is required")]
        public override string NameSN { get; set; }

        [Required(ErrorMessage = "Contractor Tamil Name is required")]
        public override string NameTA { get; set; }

        [Required(ErrorMessage = "Contractor Mailing Address is required")]
        public override string MailingAddress { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public override bool IsActive { get; set; }
    }
}
