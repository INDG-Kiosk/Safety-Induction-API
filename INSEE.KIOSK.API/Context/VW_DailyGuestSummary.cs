using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    
    public class VW_DailyGuestSummary
    {
        public string PersonName { get; set; }
        public string PersonNIC { get; set; }
        public string PersonProfileImage { get; set; }
        public string Reason { get; set; }
        public int? SiteCode { get; set; }
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        public DateTime inserted { get; set; }
        public string Company { get; set; }
        public string ExamStatus { get; set; }
        public decimal? ExamTotalMarks { get; set; }
        public DateTime? ExamCompleted { get; set; }
        public string ContractorName { get; set; }
        public int? ContractorCode { get; set; }
        public string ContractorMailingAddress { get; set; }
    }
    public class VW_GuestSummary
    {
        public string PersonName { get; set; }
        public string PersonNIC { get; set; }
        public string PersonProfileImage { get; set; }
        public string Reason { get; set; }
        public int? SiteCode { get; set; }
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        public DateTime inserted { get; set; }
        public string Company { get; set; }
        public string ExamStatus { get; set; }
        public decimal? ExamTotalMarks { get; set; }
        public DateTime? ExamCompleted { get; set; }
        public string ContractorName { get; set; }
        public int? ContractorCode { get; set; }
        public string ContractorMailingAddress { get; set; }
    }
}
