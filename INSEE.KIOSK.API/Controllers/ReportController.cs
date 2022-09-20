using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using INSEE.KIOSK.API.Context;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Authorization;

namespace INSEE.KIOSK.API.Controllers
{
    [ApiController]
    [Authorize]
    public class ReportController : Controller
    {
        private const string API_ROUTE_NAME = "api/reports";
        readonly Services.ICompanyService _companyService;
        readonly Services.ISiteService _siteService;
        readonly Services.ISettingService _settingService;
        readonly Services.ICourseService _courseService;
        readonly Services.IGuestDetailService _guestDetailService;
        readonly Services.IGuestDetailAttemptService _guestDetailAttemptService;
        readonly Services.IContractorMasterService _contractorMasterService;

        public ReportController(Services.ISettingService settingService, Services.ISiteService siteService,
            Services.ICourseService courseService, Services.IGuestDetailService guestDetailService, Services.IGuestDetailAttemptService guestDetailAttemptService, Services.ICompanyService companyService, Services.IContractorMasterService contractorMasterService)
        {
            _settingService = settingService;
            _siteService = siteService;
            _courseService = courseService;
            _guestDetailService = guestDetailService;
            _guestDetailAttemptService = guestDetailAttemptService;
            _companyService = companyService;
            _contractorMasterService = contractorMasterService;

        }


        [HttpGet]
        [Route($"{API_ROUTE_NAME}/dashboard")]
        public async Task<IActionResult> Dashboard(int code)
        {
            List<DashboardModel> models = new List<DashboardModel>();
            List<Context.VW_DailyGuestSummary> results = null;

            switch (code)
            {
                case 0:
                    results = _guestDetailAttemptService.GetAllLastAttempts(DateTime.Now, DateTime.Now);
                    break;
                case 1:
                    results = _guestDetailAttemptService.GetAllLastAttempts(DateTime.Now.AddMonths(-1), DateTime.Now);
                    break;
                case 2:
                    results = _guestDetailAttemptService.GetAllLastAttempts(DateTime.Now.AddMonths(-3), DateTime.Now);
                    break;
                case 3:
                    results = _guestDetailAttemptService.GetAllLastAttempts(DateTime.Now.AddMonths(-6), DateTime.Now);
                    break;
                default:
                    results = _guestDetailAttemptService.GetAllLastAttempts(DateTime.Now.AddYears(-1), DateTime.Now);
                    break;
            }

            var sites = _siteService.GetAll().Where(s => s.IsActive == true).ToList();

            foreach (var site in sites)
            {
                models.Add(new DashboardModel()
                {
                    SiteCode = site.Code,
                    SiteName = site.Name,
                    Location = site.Location,
                    TotalFails = results.Where(s => s.SiteCode == site.Code && s.Reason == "WORKER" && s.ExamStatus == "FAILED").Count(),
                TotalRegistered = results.Where(s => s.SiteCode == site.Code && ((s.Reason == "WORKER" && s.ExamStatus == "PASSED") || s.Reason == "VISITOR")).Count(),
                TotalVisitors = results.Where(s => s.SiteCode == site.Code && s.Reason == "VISITOR").Count(),
                TotalWorkers = results.Where(s => s.SiteCode == site.Code && s.ExamStatus == "PASSED" && s.Reason == "WORKER").Count()
            });

            }
            return Ok(new Message<List<DashboardModel>>() { Status = "S", Result = models });
        }

        [HttpGet]
        [Route($"{API_ROUTE_NAME}/dashboard/contractor/" + "{code}")]
        public async Task<IActionResult> ContarctorDetails(int code, DateTime from, DateTime to)
        {

            if (code == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Site is missing" });
            }

            DashboardModel model = new DashboardModel();
            var contactor = _contractorMasterService.GetContractorByID(code);
            List<Context.VW_DailyGuestSummary> results = _guestDetailAttemptService.GetAllLastAttempts(from, to);
            model.SiteName = contactor.NameEN;
            model.SiteCode = contactor.Code;
            model.Location = contactor.MailingAddress;
            model.TotalFails = results.Where(s => s.SiteCode == code && s.Reason == "WORKER" && s.ExamStatus == "FAILED").Count();
            model.TotalRegistered = results.Where(s => s.SiteCode == code && ((s.Reason == "WORKER" && s.ExamStatus == "PASSED") || s.Reason == "VISITOR")).Count();
            model.TotalVisitors = results.Where(s => s.SiteCode == code && s.Reason == "VISITOR").Count();
            model.TotalWorkers = results.Where(s => s.SiteCode == code && s.ExamStatus == "PASSED" && s.Reason == "WORKER").Count();
            model.Data = (from s in results
                          where s.ContractorCode == code
                          select s).ToList();

            return Ok(new Message<DashboardModel>() { Status = "S", Result = model });
        }

        [HttpGet]
        [Route($"{API_ROUTE_NAME}/dashboard/site/" + "{code}")]
        public async Task<IActionResult> Details(int code, DateTime from, DateTime to)
        {

            if (code == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Site is missing" });
            }

            DashboardModel model = new DashboardModel();
            var site = _siteService.GetSiteBySiteID(code);
            List<Context.VW_DailyGuestSummary> results = _guestDetailAttemptService.GetAllLastAttempts(from, to);
            model.SiteName = site.Name;
            model.SiteCode = site.Code;
            model.Location = site.Location;
            model.TotalFails = results.Where(s => s.SiteCode == code && s.Reason == "WORKER" && s.ExamStatus == "FAILED").Count();
            model.TotalRegistered = results.Where(s => s.SiteCode == code && ((s.Reason == "WORKER" && s.ExamStatus == "PASSED" ) || s.Reason == "VISITOR" )).Count();
            model.TotalVisitors = results.Where(s => s.SiteCode == code && s.Reason == "VISITOR" ).Count();
            model.TotalWorkers = results.Where(s => s.SiteCode == code && s.ExamStatus == "PASSED" && s.Reason == "WORKER").Count();
            model.Data = (from s in results where s.SiteCode == code
                          select s).ToList();

            return Ok(new Message<DashboardModel>() { Status = "S", Result = model });
        }

        [HttpGet]
        [Route($"{API_ROUTE_NAME}/summary/" + "{code}")]
        public async Task<IActionResult> SummaryDetailReport(int code, DateTime from, DateTime to,string type, bool export)
        {
            DashboardModel model = new DashboardModel();
            var site = _siteService.GetSiteBySiteID(code);
            List<Context.VW_DailyGuestSummary> results = _guestDetailAttemptService.GetAllLastAttempts(from, to);
            model.Data = (from s in results
                          where ((code == 0)?true: s.SiteCode == code) && ((type == "ALL")?true:s.Reason == type)
                          select s).ToList();

            return Ok(new Message<DashboardModel>() { Status = "S", Result = model });
        }

        [HttpGet]
        [Route($"{API_ROUTE_NAME}/summary/export/{{code}}")]
        public async Task<IActionResult> Export(int code, DateTime from, DateTime to, string type, bool export)
        {
            var site = _siteService.GetSiteBySiteID(code);
            List<Context.VW_DailyGuestSummary> results = _guestDetailAttemptService.GetAllLastAttempts(from, to);
            var data = (from s in results
                          where ((code == 0) ? true : s.SiteCode == code) && ((type == "ALL") ? true : s.Reason == type)
                          select s).ToList();


            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Data");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;

                //Create Headers and format them
                worksheet.Cells["A1"].Value = "Insee Kiosk Summary Report";
                using (var r = worksheet.Cells["A1:J1"])
                {
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                worksheet.Cells["A4"].Value = "Site Name";
                worksheet.Cells["B4"].Value = "Site Location";
                worksheet.Cells["C4"].Value = "Person Name";
                worksheet.Cells["D4"].Value = "Person NIC";
                worksheet.Cells["E4"].Value = "Type";
                worksheet.Cells["F4"].Value = "Contractor Name";
                worksheet.Cells["G4"].Value = "Contractor Mailing Address";
                worksheet.Cells["H4"].Value = "Pass";
                worksheet.Cells["I4"].Value = "Total Marks";
                worksheet.Cells["J4"].Value = "Pass Issued Date";
                worksheet.Cells["A4:J4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A4:J4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A4:J4"].Style.Font.Bold = true;

                row = 5;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.SiteName;
                    worksheet.Cells[row, 2].Value = item.SiteLocation;
                    worksheet.Cells[row, 3].Value = item.PersonName;
                    worksheet.Cells[row, 4].Value = item.PersonNIC;
                    worksheet.Cells[row, 5].Value = item.Reason;
                    worksheet.Cells[row, 6].Value = item.ContractorName??"";
                    worksheet.Cells[row, 7].Value = item.ContractorMailingAddress??"";
                    worksheet.Cells[row, 8].Value = (item.Reason == "VISITOR") ? "" : item.ExamStatus;
                    worksheet.Cells[row, 9].Value = (item.Reason == "VISITOR") ? "" : item.ExamTotalMarks;  
                    worksheet.Cells[row, 10].Value = (item.Reason == "VISITOR")? item.inserted.ToString() : item.ExamCompleted.ToString() ;
                    row++;
                }
   
               

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Insee Kiosk Summary";
                xlPackage.Workbook.Properties.Author = "Insee Admin";
                xlPackage.Workbook.Properties.Subject = "Insee Kiosk Summary";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InseeKioskSummary.xlsx");
        }

        [HttpGet]
        [Route($"{API_ROUTE_NAME}/site/" + "{code}/guest/{nic}")]
        public async Task<IActionResult> Details(int code, string nic)
        {

            if (code == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Site is missing" });
            }

            if (string.IsNullOrEmpty(nic))
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"nic is missing" });
            }

            GuestProfileModel model = new GuestProfileModel();
            var site = _siteService.GetSiteBySiteID(code);
            List<Context.VW_GuestSummary> results = _guestDetailAttemptService.GetAttemptDetailsByNIC(nic);
            var lastRecord = results.OrderByDescending(s => s.inserted).FirstOrDefault();

            model.SiteName = site.Name;
            model.SiteCode = site.Code;
            model.Location = site.Location;
            model.GuestName = lastRecord.PersonName;
            model.GuestNIC = lastRecord.PersonNIC;
            model.GuestImage = lastRecord.PersonProfileImage;
            model.ExamCompleted = lastRecord.ExamCompleted;
            model.ExamMark = lastRecord.ExamTotalMarks;
            model.ExamStatus = lastRecord.ExamStatus;
            model.ExamStarted = lastRecord.inserted;
            model.Reason = lastRecord.Reason;
            model.ContractorCode = lastRecord.ContractorCode??0;
            model.ContractorMailingAddress = lastRecord.ContractorMailingAddress;
            model.ContractorName = lastRecord.ContractorName;
            model.Data = results.OrderBy(s => s.inserted).ToList();
            return Ok(new Message<GuestProfileModel>() { Status = "S", Result = model });
        }

    }
}
