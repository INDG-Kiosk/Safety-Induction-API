using Microsoft.Extensions.Configuration;
using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using DinkToPdf.Contracts;
using DinkToPdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Microsoft.AspNetCore.Authorization;

namespace INSEE.KIOSK.API.Controllers
{
    [ApiController]
    public class GuestController : Controller
    {
        readonly Services.ICompanyService _companyService;
        readonly Services.IGuestDetailService _guestDetailService;
        readonly Services.IGuestMasterService _guestMasterService;
        readonly Services.IGuestDetailAttemptService _guestDetailAttemptService;
        readonly Services.ISettingService _settingService;
        readonly Services.IContractorMasterService _contractorMasterService;
        readonly Services.ISiteService _siteService;
        private static IWebHostEnvironment _webHostEnvironment;
        readonly IConfiguration _configuration;
        readonly Services.ICourseService _courseService;

        List<ValidationResult> validations;
        public GuestController(Services.ICompanyService companyService, Services.IGuestDetailService guestDetail , 
            Services.IGuestMasterService guestMaster, Services.IGuestDetailAttemptService guestDetailAttempt, 
            Services.ISettingService settingService, Services.IContractorMasterService contractorMasterService, Services.ISiteService siteService,
            IWebHostEnvironment webHostEnvironment, IConfiguration configuration, Services.ICourseService courseService)
        {
            _companyService = companyService;
            _guestDetailService = guestDetail;
            _guestMasterService = guestMaster;
            _guestDetailAttemptService = guestDetailAttempt;
            _settingService = settingService;
            _contractorMasterService = contractorMasterService;
            _siteService = siteService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _courseService = courseService;

            validations = new List<ValidationResult>();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/guests/{nic}")]
        public async Task<IActionResult> VisitorByNIC(string nic,string lang = "en", int siteCode = 0)
        {
            var video = string.Empty;
            if (string.IsNullOrEmpty(nic))
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"NIC is missing" });
            }

            try
            {
                var setting = _settingService.GetSetting();
                string code = CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString();
                string guestName = string.Empty;
                string passValidDate = string.Empty;
                List<ContractorModel> contractors = new List<ContractorModel>();

                if (setting == null || setting.PassValidPeridINMonthsForWorker == 0 || setting.ReprintValidDaysForWorker == 0 || setting.PassValidPeridINMonthsForVisitor == 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Setting is missing" });
                }

                ///Check the visitor is exists or not
                if (_guestMasterService.IsGuestExist(nic))
                {
                    ///Check the validity date of safety record
                    var lastAttempt = _guestDetailAttemptService.GetLastCompletedAttempt(nic);

                    ///Record is not exist or last attempt is fail
                    if (lastAttempt == null || CommonResources.ExamStatus.FAILED.ToString().Equals(lastAttempt.Status))
                    {
                        code = CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString();
                        guestName = lastAttempt == null ? string.Empty : lastAttempt.Guest_Detail.Guest_Master.Name;
                    }
                    else if (CommonResources.ExamStatus.PASSED.ToString().Equals(lastAttempt.Status) || lastAttempt.Guest_Detail.Type == CommonResources.GuestType.VISITOR.ToString()) /// last attempt is pass
                    {
                        int passValidPeriodInMonths = CommonResources.GuestType.WORKER.ToString().Equals(lastAttempt.Guest_Detail.Type) ?
                            setting.PassValidPeridINMonthsForWorker : setting.PassValidPeridINMonthsForVisitor;

                        if (lastAttempt.Guest_Detail.Type != CommonResources.GuestType.VISITOR.ToString())
                        {
                            DateTime passValidUntilDate = lastAttempt.TestCompletedDateTime.Value.AddMonths(passValidPeriodInMonths);
                            passValidDate = string.Format("{0:yyyy/MM/dd}", passValidUntilDate);

                            /// 2020/12/6 > 2020/12/5(Test completed date)
                            if (DateTime.Now.AddMonths(-passValidPeriodInMonths).Date > lastAttempt.TestCompletedDateTime.Value.Date)
                            {
                                code = CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString();
                                guestName = lastAttempt?.Guest_Detail.Guest_Master.Name;
                                contractors = _contractorMasterService.GetAll();
                            }
                            else if ((DateTime.Now - lastAttempt.TestCompletedDateTime).Value.Days <= setting.ReprintValidDaysForWorker) /// Check whether user can reprint ot not
                            {
                                code = CommonResources.GuestSafetyRecordStatus.REPRINT.ToString();
                            }
                            else
                            {
                                code = CommonResources.GuestSafetyRecordStatus.VALID.ToString();
                            }
                        }
                        else
                        {
                            DateTime passValidUntilDate = lastAttempt.TestStartedDateTime.AddMonths(passValidPeriodInMonths);
                            passValidDate = string.Format("{0:yyyy/MM/dd}", passValidUntilDate);

                            if (DateTime.Now.AddMonths(-passValidPeriodInMonths).Date > lastAttempt.TestStartedDateTime.Date)
                            {
                                code = CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString();
                                guestName = lastAttempt?.Guest_Detail.Guest_Master.Name;
                                contractors = _contractorMasterService.GetAll();
                            }
                            else
                            {
                                code = CommonResources.GuestSafetyRecordStatus.REPRINT.ToString();
                            }
                        }
                    }

                }

                if (CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString().Equals(code))
                {
                    contractors = _contractorMasterService.GetAll().Where(s => s.IsActive).ToList();/// Select active contractors
                }

                if (siteCode > 0)
                {
                    var course = _courseService.GetCoursBySite(siteCode);
                    if (course != null)
                    {
                        switch ((lang ?? "EN").ToLower())
                        {
                            case "en":
                                video = course.Video;
                                break;
                            case "si":
                            case "sn":
                                video = course.Video_SN;
                                if (string.IsNullOrEmpty(video))
                                    video = course.Video;
                                break;
                            case "ta":
                                video = course.Video_TA;
                                if (string.IsNullOrEmpty(video))
                                    video = course.Video;
                                break;
                        }
                    }
                    else
                    {
                        switch ((lang ?? "EN").ToLower())
                        {
                            case "si":
                            case "sn":
                                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "පාඨමාලාවක් පවරා නැත, කරුණාකර පද්ධති පරිපාලක අමතන්න" });
                            case "ta":
                                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "தளத்திற்கு பாடநெறி எதுவும் ஒதுக்கப்படவில்லை, கணினி நிர்வாகியைத் தொடர்பு கொள்ளவும்" });
                            default:
                                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "No course assigned for the site, Please contact system admin" });
                        }

                    }
                }

                return Ok(new Message<object>()
                {
                    Status = "S",
                    Code = code,
                    Result = new 
                    {
                        Name = guestName,
                        PassValidDate = passValidDate,
                        Contractors = contractors,
                        Link = video
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, new Message<string>() { Text = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/guests")]
        public async Task<IActionResult> SaveGuestDetail([FromBody] GuestModel guestModel)
        {
            var video = string.Empty;
            if (CommonResources.GuestType.WORKER.Equals(guestModel.Type) && guestModel.ContractorCode == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Contractor is missing" });
            }

            try
            {
                Guest_Master guestMaster ;
                Guest_Detail guestDetail ;
                Guest_Detail_Attempt guestDetailAttempt ;

                guestDetail = new Guest_Detail();
                guestDetail.Type = guestModel.Type;
                guestDetail.FK_SiteCode = guestModel.siteCode;
                

                if (CommonResources.GuestType.WORKER.ToString().Equals(guestModel.Type))
                {
                    guestDetail.FK_ContractorCode = guestModel.ContractorCode;
                }

                guestDetail.ProfileImg = $"{Guid.NewGuid().ToString()}.jpg";
                ///COMMENT untill SSL received
                if (guestModel.ProfileImage!= null && guestModel.ProfileImage.Length > 0)
                {

                    string path = _webHostEnvironment.WebRootPath + "\\ProfileImage\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    guestModel.ProfileImage = guestModel.ProfileImage.Replace("data:image/jpeg;base64,", string.Empty);
                    byte[] bytes = Convert.FromBase64String(guestModel.ProfileImage);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        using (FileStream fileStream = System.IO.File.Create($"{path}{guestDetail.ProfileImg}"))
                        {
                            await ms.CopyToAsync(fileStream);
                            fileStream.Flush();
                        }
                    }
                }
                else
                {
                    switch ((guestModel.Lang ?? "EN").ToLower())
                    {
                        case "si":
                        case "sn":
                            return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "පළමුව රූපය ගෙන ඉදිරියට යන්න" });
                        case "ta":
                            return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "முதலில் படத்தை எடுத்து தொடரவும்" });
                         default:
                            return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Take the image first and proceed" });
                    }
                }

                guestDetailAttempt = new Guest_Detail_Attempt();
                guestDetailAttempt.Status = string.Empty;
                guestDetailAttempt.TestStartedDateTime = DateTime.Now;
                guestDetailAttempt.Print_Count = 0;
                guestDetailAttempt.TotalMarks = 0;

                guestDetail.Guest_Detail_Attempts = new List<Guest_Detail_Attempt>();
                guestDetail.Guest_Detail_Attempts.Add(guestDetailAttempt);/// Insert guest detail attempt record

                /// Check whether Guest is exist or not.
                /// If guest is exist, insert new record to guest detail
                /// If guest is not exist, insert new record to guest master and guest detail
                if (_guestMasterService.IsGuestExist(guestModel.NIC))
                {
                    /// Get guest master id
                    guestMaster = _guestMasterService.GetGuestByNIC(guestModel.NIC);

                    if (guestMaster == null)
                    {
                        return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Master record is not available" });
                    }

                    guestDetail.FK_GuestMasterCode = guestMaster.Code;                    

                    _guestDetailService.Insert(guestDetail);/// Save guest detail record to DB
                    guestMaster.Name = guestModel.Name;
                    _guestMasterService.Update(guestMaster);
                }
                else
                {
                    guestMaster = new Guest_Master();
                    guestMaster.NIC = guestModel.NIC;
                    guestMaster.Name = guestModel.Name;
                    guestMaster.FK_CompanyCode = _siteService.GetCompanyCodeBuSiteCode(guestModel.siteCode);

                    guestMaster.Guest_Details = new List<Guest_Detail>();
                    guestMaster.Guest_Details.Add(guestDetail);

                    _guestMasterService.Insert(guestMaster);/// Save guest master record to DB       
                    

                }

 
                var course = _courseService.GetCoursBySite(guestModel.siteCode);
                if(course != null)
                {
                    switch ((guestModel.Lang ?? "EN").ToLower())
                    {
                        case "en":
                            video = course.Video;
                            break;
                        case "si":
                        case "sn":
                            video = course.Video_SN;
                            if (string.IsNullOrEmpty(video))
                                video = course.Video;
                            break;
                        case "ta":
                            video = course.Video_TA;
                            if (string.IsNullOrEmpty(video))
                                video = course.Video;
                            break;
                    }


                    return Ok(new Message<object>()
                    {
                        Status = "S",
                        Code = string.Empty,
                        Result = video
                    }); ;
                }
                else
                {
                    switch ((guestModel.Lang ?? "EN").ToLower())
                    {
                        case "si":
                        case "sn":
                            return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "පාඨමාලාවක් පවරා නැත, කරුණාකර පද්ධති පරිපාලක අමතන්න" });
                        case "ta":
                            return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "தளத்திற்கு பாடநெறி எதுவும் ஒதுக்கப்படவில்லை, கணினி நிர்வாகியைத் தொடர்பு கொள்ளவும்" });
                        default:
                            return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "No course assigned for the site, Please contact system admin" });
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, new Message<string>() { Text = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/guests/print-mobile/{nic}")]
        public async Task<IActionResult> PrintMobile(string nic)
        {
            string passValidDate = string.Empty;
            try
            {
                var setting = _settingService.GetSetting();
                var company = _companyService.GetDefualtCompany();
                ///Check the validity date of safety record
                var lastAttempt = GetLastAttempt(nic, out passValidDate);
                var print = $"[L]<b>{company.Name}</b>\n" +
                       $"[L]{lastAttempt.Guest_Detail.Site.Location}\n\n" +
                       $"[L]<b>Entry Pass</b>\n" +
                       $"[C]--------------------------------\n" +
                       $"[L]Name: {lastAttempt.Guest_Detail.Guest_Master.Name}\n" +
                       $"[C]--------------------------------\n" +
                       $"[L]NIC: {lastAttempt.Guest_Detail.Guest_Master.NIC}\n" +
                       $"[C]--------------------------------\n";

                if (lastAttempt.Guest_Detail.Type == CommonResources.GuestType.WORKER.ToString() || lastAttempt.Guest_Detail.Type == "W")
                {
                    print += $"[L]Type: { CommonResources.GuestType.WORKER.ToString()}\n";
                }
                else
                {
                    print += $"[L]Type: { CommonResources.GuestType.VISITOR.ToString()}\n";
                }
                print += $"[C]--------------------------------\n";
                print += $"[L]Valid: {passValidDate}";
                print += $"[C]---------------------------------\n\n\n";

                return Ok(new Message<object>()
                {
                    Status = "S",
                    Code = string.Empty,
                    Result = print
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, new Message<string>() { Text = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/guests/print/{nic}")]
        public async Task<IActionResult> Print(string nic)
        {
            List<ContractorModel> contractors = new List<ContractorModel>();
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            Document doc = new Document(iTextSharp.text.PageSize.A7);
            doc.SetMargins(0.0f, 0.0f, 20.0f, 0.0f);
            PdfPTable tableLayout = new PdfPTable(1);
            string code = CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString();
            string guestName = string.Empty;
            string passValidDate = string.Empty;

            try
            {
                var setting = _settingService.GetSetting();
                var company = _companyService.GetDefualtCompany();
                ///Check the validity date of safety record
                var lastAttempt = GetLastAttempt(nic, out passValidDate);// _guestDetailAttemptService.GetLastCompletedAttempt(nic);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                float[] headers = { 24, 24, 24, 24, 24, 24, 24 };

                try
                {

                    Image logo = Image.GetInstance(_webHostEnvironment.WebRootPath + "//Images//insee.png");
                    logo.ScaleAbsolute(150, 50);
                    tableLayout.AddCell(new PdfPCell(logo)
                    {
                        Colspan = 12,
                        Border = 0,
                        PaddingBottom = 5,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    });
                }
                catch (Exception ex)
                {
                }

                tableLayout.AddCell(new PdfPCell(new Phrase((company.Name ?? "Siam City Cement (Lanka) Limited"), new Font(Font.FontFamily.HELVETICA, 15, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 2,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                tableLayout.AddCell(new PdfPCell(new Phrase($"{lastAttempt.Guest_Detail.Site.Location}", new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 2,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });


                tableLayout.AddCell(new PdfPCell(new Phrase("Entry Pass", new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 2,
                    Border = 0,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("Name: {0}", lastAttempt.Guest_Detail.Guest_Master.Name), new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 1,
                    Border = 1,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("NIC   : {0}", lastAttempt.Guest_Detail.Guest_Master.NIC), new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 1,
                    Border = 1,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                if (lastAttempt.Guest_Detail.Type == CommonResources.GuestType.WORKER.ToString() || lastAttempt.Guest_Detail.Type == "W")
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("Type : {0}", CommonResources.GuestType.WORKER.ToString()), new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK)))
                    {
                        Colspan = 1,
                        Border = 1,
                        PaddingBottom = 5,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    });
                }
                else
                {

                    tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("Type : {0}", CommonResources.GuestType.VISITOR.ToString()), new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK)))
                    {
                        Colspan = 1,
                        Border = 1,
                        PaddingBottom = 5,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    });
                }

                tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("Valid : {0}", passValidDate), new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 1,
                    Border = 1,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("."), new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    Colspan = 1,
                    Border = 1,
                    PaddingBottom = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                doc.Add(tableLayout);
                doc.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return File(workStream, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, new Message<string>() { Text = ex.Message });
            }
        }


        #region Private Methods


        private Guest_Detail_Attempt GetLastAttempt(string nic,out string passValidDate)
        {
            List<ContractorModel> contractors = new List<ContractorModel>();
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            Document doc = new Document(iTextSharp.text.PageSize.A7);
            doc.SetMargins(0.0f, 0.0f, 20.0f, 0.0f);
            PdfPTable tableLayout = new PdfPTable(1);
            string code = CommonResources.GuestSafetyRecordStatus.TAKE_EXAM.ToString();
            string guestName = string.Empty;
            passValidDate = String.Empty;

            if (string.IsNullOrEmpty(nic))
            {
                throw new Exception($"NIC is missing");
            }


            var company = _companyService.GetDefualtCompany();

            var setting = _settingService.GetSetting();

            if (setting == null || setting.PassValidPeridINMonthsForWorker == 0 || setting.ReprintValidDaysForWorker == 0 || setting.PassValidPeridINMonthsForVisitor == 0)
            {
                throw new Exception("Setting is missing");
            }

            ///Check the visitor is exists or not
            if (!_guestMasterService.IsGuestExist(nic))
            {
                throw new Exception("User is missing");
            }

            ///Check the validity date of safety record
            var lastAttempt = _guestDetailAttemptService.GetLastCompletedAttempt(nic);

            ///Record is not exist or last attempt is fail
            if (lastAttempt == null || CommonResources.ExamStatus.FAILED.ToString().Equals(lastAttempt.Status))
            {
                throw new Exception("Invalid Request");
            }
            else if (CommonResources.ExamStatus.PASSED.ToString().Equals(lastAttempt.Status) || lastAttempt.Guest_Detail.Type == CommonResources.GuestType.VISITOR.ToString()) /// last attempt is pass
            {
                int passValidPeriodInMonths = CommonResources.GuestType.WORKER.ToString().Equals(lastAttempt.Guest_Detail.Type) ?
                    setting.PassValidPeridINMonthsForWorker : setting.PassValidPeridINMonthsForVisitor;

                if (lastAttempt.Guest_Detail.Type != CommonResources.GuestType.VISITOR.ToString())
                {
                    //WORKER
                    DateTime passValidUntilDate = lastAttempt.TestCompletedDateTime.Value.AddMonths(passValidPeriodInMonths);
                    passValidDate = string.Format("{0:yyyy/MM/dd}", passValidUntilDate);

                    /// 2020/12/6 > 2020/12/5(Test completed date)
                    if (DateTime.Now.AddMonths(-passValidPeriodInMonths).Date > lastAttempt.TestCompletedDateTime.Value.Date)
                    {
                        throw new Exception("Invalid Request");
                    }
                }
                else
                {
                    /// VISITOR
                    DateTime passValidUntilDate = lastAttempt.TestStartedDateTime.AddMonths(passValidPeriodInMonths);
                    passValidDate = string.Format("{0:yyyy/MM/dd}", passValidUntilDate);

                    if (DateTime.Now.AddMonths(-passValidPeriodInMonths).Date > lastAttempt.TestStartedDateTime.Date)
                    {
                        throw new Exception("Invalid Request");
                    }
                }
            }
            return lastAttempt;
        }

        private  string GetHTMLString()
        {
           
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>LastName</th>
                                        <th>Age</th>
                                        <th>Gender</th>
                                    </tr>");
            
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", "gevan", "Jayasinghe","nimal", "Male");
            
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
        private string SaveImage(string base64Image)
        {
            var fileName = string.Format(@"{0}", Guid.NewGuid());

            //Generate unique filename
            string filepath = "~/GuestImages/" + fileName + ".jpeg";
            var bytess = Convert.FromBase64String(base64Image);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(bytess, 0, bytess.Length);
                imageFile.Flush();
            }

            return fileName;
        }

        #endregion
    }
}
