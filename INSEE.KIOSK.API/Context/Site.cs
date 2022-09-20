using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("M_Site")]
    public class  Site
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(16)]
        public string IP { get; set; }

        [Required]
        public string ResourcePath { get; set; }

        /// Foreign Keys
        [Required]
        public int FK_CompanyCode { get; set; }
        [ForeignKey("FK_CompanyCode")]
        public virtual Company Company { get; set; }

        public string Location { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

       
        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime ModifiedDateTime { get; set; } = DateTime.Now;

        ///relations
       public virtual ICollection<Guest_Detail> Guest_Details { get; set; }
       public virtual ICollection<Log> Logs { get; set; }
       public virtual ICollection<Course> Courses { get; set; }
    }
}
