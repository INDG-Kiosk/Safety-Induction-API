using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("T_Guest_Master")]
    public class Guest_Master
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string NIC { get; set; }

        /// Foreign Keys
        [Required]
        [Column("FK_CompanyCode")]
        public int FK_CompanyCode { get; set; }

        [ForeignKey("FK_CompanyCode")]
        public virtual Company Company { get; set; }

        [Required]
        public DateTime InsertedDateTime { get; set; } 

        ///relations
        public virtual ICollection<Guest_Detail> Guest_Details { get; set; }
    }
}
