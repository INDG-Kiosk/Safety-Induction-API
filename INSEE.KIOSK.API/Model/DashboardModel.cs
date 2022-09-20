using INSEE.KIOSK.API.Context;
using System;
using System.Collections.Generic;

namespace INSEE.KIOSK.API.Model
{

    public class DashboardModel
    {
        public int SiteCode { get; set; }
        public string SiteName { get; set; }
        public string Location { get; set; }
        public int TotalRegistered { get; set; }
        public int TotalVisitors { get; set; }
        public int TotalWorkers { get; set; }
        public int TotalFails { get; set; }
        public List<VW_DailyGuestSummary> Data { get; set; }
        public string ContractorName { get; set; }
        public int ContractorCode { get; set; }
        public string ContractorMailingAddress { get; set; }
    }

    public class GuestProfileModel
    {
        public int SiteCode { get; set; }
        public string SiteName { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public string GuestName { get; set; }
        public string GuestNIC { get; set; }
        public string GuestImage { get; set; }
        public string? ExamStatus { get; set; }
        public decimal? ExamMark { get; set; }
        public DateTime ExamStarted { get; set; }
        public DateTime? ExamCompleted { get; set; }
        public int PassPrintCount { get; set; }
        public List<VW_GuestSummary> Data { get; set; }
        public string ContractorName { get; set; }
        public int ContractorCode { get; set; }
        public string ContractorMailingAddress { get; set; }
    }
}
