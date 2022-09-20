using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
   
   [Table("M_Settings")]
    public class Settings
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        public int ReprintValidDaysForWorker { get; set; }

        [Required]
         public int PassValidPeridINMonthsForWorker { get; set; }


        [Required]
        public int PassValidPeridINMonthsForVisitor { get; set; }

        /// Foreign Keys
        [Required]
        public int FK_CompanyCode { get; set; }
        [ForeignKey("FK_CompanyCode")]
        public virtual Company Company { get; set; }

     
        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime Modified_DateTime { get; set; }

        ///relations
      

    }
}
