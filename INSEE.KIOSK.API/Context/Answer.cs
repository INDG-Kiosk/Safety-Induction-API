using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{

    [Table("M_Answer")]
    public class Answer
    {
        [Key, Column("Code"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required]
        public string TextEN { get; set; }
        [Required]
        public string TextSN { get; set; }
        [Required]
        public string TextTA { get; set; }

        [Required]
        public bool IsCorrect { get; set; } = false;

        /// Foreign Keys
        [Required]
        public int FK_QuestionCode { get; set; }

        [ForeignKey("FK_QuestionCode")]
        public virtual Question Question { get; set; }

    
        [Column("ModifiedBy", TypeName = "nvarchar(450)")]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUser User { get; set; }
        
        [Required]
        public DateTime ModifiedDateTime { get; set; }

        ///relations
    }
}
