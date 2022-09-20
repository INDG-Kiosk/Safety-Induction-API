using System.ComponentModel.DataAnnotations;

namespace INSEE.KIOSK.API.Model
{
    public class SiteModel
    {
        public virtual int Code { get; set; }
        public virtual string Name { get; set; } = string.Empty;
        public virtual string IPAddress { get; set; } = string.Empty;
        public virtual string ResourcePath { get; set; } = string.Empty;
        public virtual string Location { get; set; } = string.Empty;
        public virtual bool IsActive { get; set; } = false;
    }

    public class InsertSiteModel : SiteModel
    {

        [Required(ErrorMessage = "Location is required")]
        public override string Location { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public override bool IsActive { get; set; }
    }

    public class UpdateSiteModel : SiteModel
    {
        [Required(ErrorMessage = "Site Code is required")]
        public override int Code { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public override string Location { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public override bool IsActive { get; set; }
    }
}
