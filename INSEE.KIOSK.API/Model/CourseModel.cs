using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    public class CourseModel
    {
        public virtual int Code { get; set; } 
        public virtual string Title { get; set; } 
        public virtual decimal PassRate { get; set; } =0.0m;
        public virtual string Video { get; set; } = string.Empty;
        public virtual string Video_SN { get; set; } = string.Empty;
        public virtual string Video_TA { get; set; } = string.Empty;
        public virtual bool IsActive { get; set; } = false;

        public virtual int SiteCode { get; set; }
       
    }

    public class DisplayCourseModel : CourseModel
    {
        [Display]
        public string SiteName { get; set; }
        [Display]
        public string Location { get; set; }
    }

    public class InsertCourseModel : CourseModel
    {
        [Required(ErrorMessage = "Title is required")]
        public override string Title { get; set; }

        [Required(ErrorMessage = "Site is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Site is required")]
        public override int SiteCode { get; set; }

        //[Required(ErrorMessage = "Video is required")]
        public override string Video { get; set; }

        //[Required(ErrorMessage = "Contractor Tamil Name is required")]
        //public override string NameTA { get; set; }

        //[Required(ErrorMessage = "Contractor Mailing Address is required")]
        //public override string MailingAddress { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public override bool IsActive { get; set; }
    }

    public class UpdateCourseModel : CourseModel
    {
        [Required(ErrorMessage = "Contractor Code is required")]
        public override int Code { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public override string Title { get; set; }

        //[Required(ErrorMessage = "Video is required")]
        public override string Video { get; set; }

        [Required(ErrorMessage = "Site is required")]
        public override int SiteCode { get; set; }

        //[Required(ErrorMessage = "Contractor Tamil Name is required")]
        //public override string NameTA { get; set; }

        //[Required(ErrorMessage = "Contractor Mailing Address is required")]
        //public override string MailingAddress { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public override bool IsActive { get; set; }
    }
}
