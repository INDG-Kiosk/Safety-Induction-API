using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
 
    [Table("M_Course")]
    public class Course
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        public string Title { get; set; } = ""; 

        [Required]
        [MaxLength(200)]
        public string Video { get; set; } /// <VideoName>_EN / _SN / _TA
      
        [MaxLength(200)]
        public string Video_SN { get; set; } 
    
        [MaxLength(200)]
        public string Video_TA { get; set; } 
        
        [Required]
        [Column(TypeName = "decimal(16,4)")]
        public decimal PassRate { get; set; } = 0;

        //[Required]
        //public int NumberOfQuestion { get; set; } = 0; // can take assign Question Count

        [Required]
        public bool IsActive { get; set; } = true;

        /// Foreign Keys
        //[Required]
        public int FK_SiteCode { get; set; }

        [ForeignKey("FK_SiteCode")]
        public virtual Site Site { get; set; }

        
        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }
  
        [Required]
        public DateTime ModifiedDateTime { get; set; }

        ///relations
        public virtual ICollection<Course_Question> Course_Questions { get; set; }
    }
}
