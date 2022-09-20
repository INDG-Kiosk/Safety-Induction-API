using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface IQuestionService
    {
        public Message<string> Insert(Question question);
        public Message<string> Update(UpdateQuestionModel question);

        public List<QuestionModel> GetAll();
        public Question GetQuestionByID(int code);
    }

    public class QuestionService : IQuestionService
    {
        readonly ApplicationDbContext _appdDbContext;
        public QuestionService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public Message<string> Insert(Question question)
        {
            _appdDbContext.Questions.Add(question);
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"New Question Is Saved", Status = "S" };
        }

        public Message<string> Update(UpdateQuestionModel question)
        {
            //TODO: COMMENT BEFORE GO LIVE
            //TODO: Need to add User ID
            var result = _appdDbContext.Questions.SingleOrDefault(s => s.Code == question.Code);

            if (result == null)
                return new Message<string>() { Text = $"Question is not found" };            

            result.IsActive = question.IsActive;
            result.ModifiedBy = question.LastUpdated;
            result.ModifiedDateTime = DateTime.Now;
            result.TextEN = question.TextEN;
            result.TextSN = question.TextSN;
            result.TextTA = question.TextTA;

            foreach (var answer in question.Answers)
            {
                var newAnswer = result.Answers.FirstOrDefault(s => s.Code == answer.Code);
                if (newAnswer != null)
                {
                    newAnswer.TextEN = answer.TextEN;
                    newAnswer.TextSN = answer.TextSN;
                    newAnswer.TextTA = answer.TextTA;
                    newAnswer.IsCorrect = answer.IsCorrectAnswer;
                    newAnswer.ModifiedBy = question.LastUpdated;
                    newAnswer.ModifiedDateTime = DateTime.Now;
                }
            }
            _appdDbContext.SaveChanges();
            return new Message<string>() { Text = $"Question Updated Successfully", Status = "S" };
        }

        public List<QuestionModel> GetAll()
        {
            var results = _appdDbContext.Questions
                .Include(s => s.Answers)
                .OrderBy(s => s.ModifiedDateTime)
                .Select(s => new QuestionModel
                {
                    Code = s.Code,
                    TextEN = s.TextEN,
                    TextSN = s.TextSN,
                    TextTA = s.TextTA,
                    IsActive = s.IsActive,
                    Answers = s.Answers.Select(a => new AnswerModel
                    {
                        Code = a.Code,
                        TextEN = a.TextEN,
                        TextSN = a.TextSN,
                        TextTA = a.TextTA,
                        IsCorrectAnswer = a.IsCorrect
                    }).ToList()

                }).ToList();

            return results;
        }

        public Question GetQuestionByID(int code)
        {
            var result = _appdDbContext.Questions
                           .Include(s => s.Answers)
                         .SingleOrDefault(s => s.Code == code);
            return result;
        }
    }
}


