using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
   [Table("T_Log")]
    public class Log
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime InsertedDateTime { get; set; }

        /// Foreign Keys
        public int? FK_SiteCode { get; set; }
        [ForeignKey("FK_SiteCode")]
        public virtual Site Site { get; set; }

        ///relations
    }
}
