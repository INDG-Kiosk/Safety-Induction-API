using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("T_Guest_Detail")]
    public class Guest_Detail
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        [MaxLength(10)]
        public string Type { get; set; }

        [Required]
        public string ProfileImg { get; set; }
   
        /// Foreign Keys
        public int? FK_SiteCode { get; set; }
        [ForeignKey("FK_SiteCode")]
        public virtual Site Site { get; set; }

     
        public int FK_GuestMasterCode { get; set; }
        [ForeignKey("FK_GuestMasterCode")]
        public virtual Guest_Master Guest_Master { get; set; }

        public int? FK_ContractorCode { get; set; }
        [ForeignKey("FK_ContractorCode")]
        public virtual Contractor_Master Contractor_Master { get; set; }

        [Required]
        public DateTime InsertedDateTime { get; set; } = DateTime.Now;

        ///relations
         public virtual ICollection<Guest_Detail_Attempt> Guest_Detail_Attempts { get; set; }
    }
}
