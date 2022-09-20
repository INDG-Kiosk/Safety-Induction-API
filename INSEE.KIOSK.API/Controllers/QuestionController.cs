using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Controllers
{
     [Authorize]
    //  [Authorize(Roles = "Administrator")]
    [ApiController]
    public class QuestionrController : Controller
    {
        private const string API_ROUTE_NAME = "api/questions";

        readonly Services.IQuestionService _questionService;
        readonly Services.ICompanyService _companyService;
        List<ValidationResult> validations;
        public QuestionrController(Services.IQuestionService questionService, Services.ICompanyService companyService)
        {
            _questionService = questionService;
            _companyService = companyService;
            validations = new List<ValidationResult>();
        }

        [HttpGet]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var questions = _questionService.GetAll();

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

        [HttpGet]
        [Route(API_ROUTE_NAME + "/{id}")]
        public async Task<IActionResult> GetQuestionByID(int id)
        {
            if (id == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Your request is missing parameters" });
            }

            try
            {
                var question = _questionService.GetQuestionByID(id);

                if (question == null)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Invalid Question ID" });

                var result = new Model.QuestionModel()
                {
                    Code = question.Code,
                    TextEN = question.TextEN,
                    TextSN = question.TextSN,
                    TextTA = question.TextTA,
                    IsActive = question.IsActive,
                    Answers = question.Answers.Select(a => new AnswerModel
                    {
                        Code = a.Code,
                        TextEN = a.TextEN,
                        TextSN = a.TextSN,
                        TextTA = a.TextTA,
                        IsCorrectAnswer = a.IsCorrect
                    }).ToList()
                };

                return Ok(new Message<QuestionModel>()
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
        //  [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Insert([FromBody] Model.InsertQuestionModel model)
        {
            try
            {
                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("Id").Value;

                if (!model.Answers.Any())
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Answers are not found!" });

                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add USER ID

                var answers = new List<Answer>();

                foreach (var item in model.Answers)
                {
                    answers.Add(new Answer
                    {
                        TextEN = item.TextEN,
                        TextSN = item.TextSN,
                        TextTA = item.TextTA,
                        IsCorrect = item.IsCorrectAnswer,
                        ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                    ModifiedDateTime = DateTime.Now,
                    });
                }

                var question = new Question()
                {
                    TextEN = model.TextEN,
                    TextSN = model.TextSN,
                    TextTA = model.TextTA,
                    IsActive = true,
                    ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                ModifiedDateTime = DateTime.Now,
                    Answers = answers
                };

                return Ok(_questionService.Insert(question));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPost]
        [Route(API_ROUTE_NAME + "/all")]
        //  [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> InsertAll([FromBody] List<Model.InsertQuestionModel> model)
        {
            try
            {
                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("Id").Value;

                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add USER ID

                foreach (var q in model)
                {
                    if (!q.Answers.Any())
                        return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Answers are not found!" });

                    var answers = new List<Answer>();

                    foreach (var item in q.Answers)
                    {
                        answers.Add(new Answer
                        {
                            TextEN = item.TextEN,
                            TextSN = item.TextSN,
                            TextTA = item.TextTA,
                            IsCorrect = item.IsCorrectAnswer,
                            ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                        ModifiedDateTime = DateTime.Now,
                        });
                    }

                    var question = new Question()
                    {
                        TextEN = q.TextEN,
                        TextSN = q.TextSN,
                        TextTA = q.TextTA,
                        IsActive = true,
                        ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                    ModifiedDateTime = DateTime.Now,
                        Answers = answers
                    };

                    _questionService.Insert(question);
                }

                return Ok(new Message<string>() { Text = $"New Question Is Saved", Status = "S" });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPut]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> Update([FromBody] Model.UpdateQuestionModel model)
        {
            try
            {
                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User ID
                var question = _questionService.GetQuestionByID(model.Code);

                if (question == null)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Invalid Question ID" });

                model.LastUpdated = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
                var result = _questionService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPut]
        [Route(API_ROUTE_NAME + "/all")]
        public async Task<IActionResult> UpdateAll([FromBody] List<Model.UpdateQuestionModel> model)
        {
            try
            {
                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User ID

                foreach (var item in model)
                {
                    var result = _questionService.Update(item);
                }

                return Ok(new Message<string>() { Text = $"Questions is updated", Status = "S" });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }
    }
}
