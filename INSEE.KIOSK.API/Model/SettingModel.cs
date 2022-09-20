using System.ComponentModel.DataAnnotations;

namespace INSEE.KIOSK.API.Model
{
    public class SettingModel
    {
        public int Code { get; set; }

        [Required]
        public int ReprintValidDaysForWorker { get; set; }

        [Required]
        public int PassValidPeridINMonthsForWorker { get; set; }

        [Required]
        public int PassValidPeridINMonthsForVisitor { get; set; }

    }
}
