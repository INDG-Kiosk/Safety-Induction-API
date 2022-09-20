using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Controllers
{
    [ApiController]
  //  [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrator")]
    public class SiteController : Controller
    {
        private const string API_ROUTE_NAME = "api/sites";

        readonly Services.ICompanyService _companyService;
        readonly Services.ISiteService _siteService;
        readonly Services.ISettingService _settingService;
        readonly Services.ICourseService _courseService;
        readonly Services.IGuestDetailService _guestDetailService;
        readonly Services.IGuestDetailAttemptService _guestDetailAttemptService;


        List<ValidationResult> validations;
        public SiteController(Services.ISettingService settingService, Services.ISiteService kioskService,
            Services.ICourseService courseService, Services.IGuestDetailService guestDetailService, Services.IGuestDetailAttemptService guestDetailAttemptService, Services.ICompanyService companyService)
        {
            _settingService = settingService;
            _siteService = kioskService;
            _courseService = courseService;
            _guestDetailService = guestDetailService;            
            _guestDetailAttemptService = guestDetailAttemptService;
            _companyService = companyService;

            validations = new List<ValidationResult>();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(API_ROUTE_NAME +"/{id}/questions")]
        public async Task<IActionResult> GetQuestionsBySites(int id = 0, string lang = "en")
        {
            if (id <= 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Site id is missing" });
            }

            try
            {
                var questions = _courseService.GetCourseQuestionsBySite(id, lang);

                ///If question's count is greater than zero, shuffle the questions and answers
                if (questions.Count > 0)
                {
                    Random rng = new Random();
                    questions = questions.OrderBy(a => rng.Next()).ToList();///Shuffle questions

                    foreach (var item in questions)
                    {
                        item.Answers = item.Answers.OrderBy(a => rng.Next()).ToList(); ///Shuffle answers
                    }
                }

                return Ok(new Message<List<ExamQuestionModel>>()
                {
                    Status = "S",
                    Result = questions
                });

            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, new Message<string>() { Text = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route(API_ROUTE_NAME + "/{id}/questions")]
        public async Task<IActionResult> SaveExamAttempt(int id, [FromBody] ExamQuestionAnswerPostModel questionAnswerModel)
        {
            if (id <= 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Sites id is missing" });
            }

            try
            {
                int correctAnswers = 0;
                var courseDetails = _courseService.GetCourseByKiosk(id);
                var lastStartedAttempt = _guestDetailAttemptService.GetLastStartedAttempt(questionAnswerModel.NIC);

                if (courseDetails == null)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Course detail is empty" });
                }
                else if (courseDetails.PassRate <= 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Course pass rate is zero" });
                }
                else if (courseDetails.Course_Questions.Count == 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Course questions are empty" });
                }
                else if (lastStartedAttempt == null)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Attempt is empty" });
                }

                ///Check whether given answer is correct or not
                foreach (var item in questionAnswerModel.QuestionAnswers)
                {
                    /// If answer is correct, increase correct answer count
                    if (_courseService.IsQuestionAnswerCorrect(item.Question, item.Answer))
                    {
                        correctAnswers++;
                    }
                }

                ///Calculate exam mark
                decimal examMarks = ((decimal)correctAnswers / courseDetails.Course_Questions.Count) * 100;
                string examResult = CommonResources.ExamStatus.FAILED.ToString();

                ///Check whether exam is pass or fail
                if (examMarks >= courseDetails.PassRate)
                {
                    examResult = CommonResources.ExamStatus.PASSED.ToString();
                }

                ///Update guest detail attemp record
                lastStartedAttempt.Status = examResult;
                lastStartedAttempt.TestCompletedDateTime = DateTime.Now;
                lastStartedAttempt.TotalMarks = examMarks;
                lastStartedAttempt.Print_Count = 1;

                ///Update guest detail attemp to DB
                _guestDetailAttemptService.Update(lastStartedAttempt);

                return Ok(new Message<string>()
                {
                    Status = "S",
                    Result = examResult
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, new Message<string>() { Text = ex.Message });
            }
        }

        //  [Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [HttpGet]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(new Message<List<SiteModel>> { Status = "S", Result = _siteService.GetAll() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route(API_ROUTE_NAME + "/{id}")]
        public async Task<IActionResult> GetSiteDetailByID(int id)
        {
            try
            {
                var site = _siteService.GetSiteBySiteID(id);
                var siteModel = new SiteModel()
                {
                    Code = site.Code,
                    IPAddress = site.IP,
                    IsActive = site.IsActive,
                    Location = site.Location,
                    Name = site.Name,
                    ResourcePath = site.ResourcePath
                };
                return Ok(new Message<SiteModel> { Status = "S", Result = siteModel });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> Insert([FromBody] Model.InsertSiteModel model)
        {
            try
            {
                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("Id").Value;

                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User Id
                return Ok(_siteService.Insert(new Site()
                {
                    Name = model.Name,
                    IP = model.IPAddress,
                    Location = model.Location,
                    ResourcePath = model.ResourcePath,
                    FK_CompanyCode = _companyService.GetDefualtCompany().Code,
                    IsActive = true,
                    ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                    ModifiedDateTime = DateTime.Now
                }));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> Update([FromBody] Model.UpdateSiteModel model)
        {
            try
            {
                var site = _siteService.GetSiteBySiteID(model.Code);

                if (site == null)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Invalid Site ID" });

                site.Name = model.Name;
                site.Location = model.Location;
                site.ResourcePath = model.ResourcePath;
                site.IsActive = model.IsActive;
                site.IP = model.IPAddress;
                site.ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
                site.ModifiedDateTime = DateTime.Now;
                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User Id
                return Ok(_siteService.Update(site))
                    ;
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }



    }
}
