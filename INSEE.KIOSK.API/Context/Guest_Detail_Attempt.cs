using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
   [Table("T_Guest_Detail_Attempt")]
    public class Guest_Detail_Attempt
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        public DateTime TestStartedDateTime { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "INIT";

        [Required]
        [Column(TypeName = "decimal(16,4)")]
        public decimal TotalMarks { get; set; } = 0;

        [Required]
        public int Print_Count { get; set; } = 0;
        public DateTime? TestCompletedDateTime { get; set; }

        /// Foreign Keys
        [Required]
        public int FK_GuestDetailCode { get; set; }
        [ForeignKey("FK_GuestDetailCode")]
        public virtual Guest_Detail Guest_Detail { get; set; }

       
        ///relations
    }
}
