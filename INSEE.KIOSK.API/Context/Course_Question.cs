using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    [Table("M_Course_Question")]
    public class Course_Question
    {
        /// Foreign Keys
        [Required]
        [Key,  Column(Order = 0)]
        public int FK_QuestionCode { get; set; }
        [ForeignKey("FK_QuestionCode")]
        public virtual Question Question { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public int FK_CourseCode { get; set; }
        [ForeignKey("FK_CourseCode")]
        public virtual Course Course { get; set; }


        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime ModifiedDateTime { get; set; } = DateTime.Now;

        ///relations
      
    }
}
