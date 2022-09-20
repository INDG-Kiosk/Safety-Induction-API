using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("M_Contractor_Master")]
    public class Contractor_Master
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string NameEN { get; set; }

        [Required]
        [MaxLength(200)]
        public string NameSN { get; set; }

        [Required]
        [MaxLength(200)]
        public string NameTA { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
      
        public string MailingAddress { get; set; } = string.Empty;

        /// Foreign Keys

        public int FK_CompanyCode { get; set; }
        [ForeignKey("FK_CompanyCode")]
        public virtual Company Company { get; set; }


        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime ModifiedDateTime { get; set; }

        ///relations
        public virtual ICollection<Guest_Detail> Guest_Details { get; set; }
    }
}
