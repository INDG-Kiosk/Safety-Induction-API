using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("M_Company")]
    public class Company
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; } = "LK";

        [Required]
        public bool IsActive { get; set; } = true;

     
        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime ModifiedDateTime { get; set; }

        ///relations
       public virtual  ICollection<Site> Sites { get; set; }
        public virtual  ICollection<Contractor_Master> Contractors_Master { get; set; }
        public virtual  ICollection<Guest_Master> Guest_Master { get; set; }
        public virtual Settings Settings { get; set; }

    }
}
