using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("M_Location")]
    public class Location
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }

        /// Foreign Keys
      //  [Required]
      //  public int FK_CompanyCode { get; set; }

       // [ForeignKey("FK_CompanyCode")]
      //  public virtual Company Company { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

      
        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }
        public DateTime ModifiedDateTime { get; set; } = DateTime.Now;

        ///relations
        //public virtual ICollection<Kiosk> Kiosks { get; set; }
    }
}
