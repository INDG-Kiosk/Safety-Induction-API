using Microsoft.Extensions.Configuration;
using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using INSEE.KIOSK.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Controllers
{
    [Authorize]
    //  [Authorize(Roles = "Administrator")]
    [ApiController]
    public class CourseController : Controller
    {
        private const string API_ROUTE_NAME = "api/courses";

        readonly Services.ICourseService _courseService;
        readonly Services.ISiteService _siteService;
        private static IWebHostEnvironment _webHostEnvironment;
        List<ValidationResult> validations;
        readonly IConfiguration _configuration;


        public CourseController(Services.ICourseService courseService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration,ISiteService siteService)
        {
            _courseService = courseService;
            validations = new List<ValidationResult>();
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _siteService = siteService;
        }

       

        [HttpGet]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // var userID = User.Identities.First().Claims.Single(s=>s.Type == "uid").Value;
                var courses = _courseService.GetAll();
                return Ok(new Message<List<DisplayCourseModel>>()
                {
                    Status = "S",
                    Result = courses
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpGet]
        [Route(API_ROUTE_NAME + "/{id}")]
        public async Task<IActionResult> GetCourseByID(int id)
        {
            if (id == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Course ID is missing" });
            }

            try
            {
                var course = _courseService.GetCourseByID(id);

                if (course == null)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Invalid Course ID" });
                }

                var result = new Model.CourseModel()
                {
                    Code = course.Code,
                    Video = course.Video,
                    Video_TA = course.Video_TA,
                    Video_SN = course.Video_SN,
                    PassRate = course.PassRate,
                    IsActive = course.IsActive,
                    Title = course.Title,
                    SiteCode = course.FK_SiteCode

                };

                return Ok(new Message<CourseModel>()
                {
                    Status = "S",
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPost]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> Insert([FromBody] Model.InsertCourseModel model)
        {
            try
            {
                if (model.SiteCode > 0 && model.IsActive)
                {
                    var site = _siteService.GetSiteBySiteID(model.SiteCode);
                    var existCourse = site.Courses.Where(s => s.IsActive == true).FirstOrDefault();
                    if (existCourse != null)
                    {
                        return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"{site.Name} already assigned to course \"{existCourse.Title}\", please inactive existing course" });
                    }
                }

                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("Id").Value;

                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User Id
                var course = new Course()
                {
                    Video = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    PassRate = model.PassRate,
                    IsActive = true,
                    ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                    ModifiedDateTime = DateTime.Now
                };

                if(model.SiteCode > 0)
                {
                    course.FK_SiteCode = model.SiteCode;
                }
                else
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Please Select a Site" });
                }
                return Ok(_courseService.Insert(course));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPut]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> Update([FromBody] Model.UpdateCourseModel model)
        {
            try
            {
                if(model.SiteCode > 0 && model.IsActive)
                {
                    var site = _siteService.GetSiteBySiteID(model.SiteCode);
                    var existCourse = site.Courses.Where(s=>s.IsActive == true).FirstOrDefault();
                    if(existCourse!= null && existCourse.Code != model.Code)
                    {
                        return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"{site.Name} already assigned to course {existCourse.Title}, please inactive existing course"});
                    }
                }

                var course = _courseService.GetCourseByID(model.Code);

                if (course == null)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Invalid Course ID" });

                //course.Video = model.Title;
                course.Title = model.Title;
                course.PassRate = model.PassRate;
                course.FK_SiteCode = model.SiteCode;
                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User Id
                course.ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
                course.ModifiedDateTime = DateTime.Now;
                course.IsActive = model.IsActive;

                var result = _courseService.Update(course);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }


        [HttpGet]
        [Route(API_ROUTE_NAME + "/{id}/questions")]
        public async Task<IActionResult> GetQuestionsByCourseID(int id)
        
        {
            try
            {
                if (id == 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Course ID is missing." });

                var questions = _courseService.GetQuestionByCourseID(id);

                if (questions == null)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Invalid Course ID" });
                }

                return Ok(new Message<List<QuestionModel>>()
                {
                    Status = "S",
                    Result = questions
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }       

        [HttpPost]
        [Route(API_ROUTE_NAME + "/{id}/questions")]
        public async Task<IActionResult> PostQuestionsByCourseID(int id, List<int> questions)
        {
            try
            {
                if (id == 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Course ID is missing." });

                if (questions == null || !questions.Any())
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Questions are missing" });

                // Update course questions
                _courseService.UpdateQuestionsByCourse(id, questions);

                return Ok(new Message<string>() { Text = $"Course questions updated", Status = "S" });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPost]
        [Route(API_ROUTE_NAME + "/{id}/videos")]
        //[RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        //[RequestSizeLimit(509715200)]
        //[DisableRequestSizeLimit]
        public async Task<IActionResult> PostVideoByCourseID(int id, string lang, [FromForm] VideoUpload videoFile)
        {
            try
            {
                string video = string.Empty;
                var folderPath = _webHostEnvironment.WebRootPath + "\\Uploads\\";
                if (id == 0)
                    return Ok(new Message<string>() { Text = $"Course ID is missing." });

                if (string.IsNullOrEmpty(lang))
                    return Ok(new Message<string>() { Text = $"Language is missing" });

                var course = _courseService.GetCourseByID(id);
                switch (lang.ToLower())
                {
                    case "en":
                        course.Video = $"{Guid.NewGuid().ToString()}_{lang}.{videoFile.Files.FileName.Split('.').Last()}";
                        video = $"{course.Video}";
                        break;
                    case "si":
                    case "sn":
                        course.Video_SN = $"{Guid.NewGuid().ToString()}_{lang}.{videoFile.Files.FileName.Split('.').Last()}";
                        video = $"{course.Video_SN}";
                        break;
                    case "ta":
                        course.Video_TA = $"{Guid.NewGuid().ToString()}_{lang}.{videoFile.Files.FileName.Split('.').Last()}";
                        video = $"{course.Video_TA}";
                        break;
                    default:
                        return Ok(new Message<string>() { Text = $"Invalid Lang" });
                }

                if (videoFile.Files.Length > 0)
                {

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (FileStream fileStream = System.IO.File.Create($"{folderPath}{video}"))
                    {
                        await videoFile.Files.CopyToAsync(fileStream);
                        fileStream.Flush();
                    }
                    /// update course details
                    course.ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
                    course.ModifiedDateTime = DateTime.Now;
                    _courseService.Update(course);
                }

                return Ok(new Message<string>() { Text = $"Course video uploaded", Result = video, Status = "S" });
            }
            catch (Exception ex)
            {
                return Ok(new Message<string>() { Text = ex.Message });
            }
        }

        #region Private Methods

        private string SaveVideo(string base64Image)
        {
            var fileName = string.Format(@"{0}", Guid.NewGuid());

            //Generate unique filename
            string filepath = "~/GuestImages/" + fileName + ".mp4";
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
