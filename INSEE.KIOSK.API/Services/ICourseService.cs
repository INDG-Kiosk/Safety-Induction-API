using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface ICourseService
    {
        public Message<string> Insert(Course course);
        public Message<string> Update(Course course);

        public Course GetCourseByKiosk(int siteCode);
        public decimal GetCoursePassRateByKiosk(int siteCode);
        public List<ExamQuestionModel> GetCourseQuestionsBySite(int siteCode, string lang);
        public bool IsQuestionAnswerCorrect(int questionCode, int answerCode);

        public List<DisplayCourseModel> GetAll();

        public Course GetCourseByID(int code);

        public List<QuestionModel> GetQuestionByCourseID(int courseId);
        public void UpdateQuestionsByCourse(int courseId, List<int> questions);
        public Course GetCoursBySite(int siteCode);
    }

    public class CourseService : ICourseService
    {
        readonly ApplicationDbContext _appdDbContext;
        public CourseService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public Message<string> Insert(Course course)
        {
            _appdDbContext.Courses.Add(course);
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"New Course {course.Title} Registered", Status = "S", Result = course.Code.ToString() };
        }

        public Message<string> Update(Course course)
        {
            var result = _appdDbContext.Courses.SingleOrDefault(s => s.Code == course.Code);
            if (result == null)
            {
                return new Message<string>() { Text = $"Course  {course.Title} Not Found" };
            }

            result.ModifiedBy = course.ModifiedBy;
            result.ModifiedDateTime = course.ModifiedDateTime;
            result.IsActive = course.IsActive;
            result.PassRate = course.PassRate;
            result.Video = course.Video;
            result.Title = course.Title;
            result.Video_SN = course.Video_SN;
            result.Video_TA = course.Video_TA;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"Course Detail {course.Title} Successfully Updated", Status = "S" };
        }

        public decimal GetCoursePassRateByKiosk(int siteCode)
        {
            var result = _appdDbContext.Courses
                  .Where(s => s.FK_SiteCode == siteCode)
                  .FirstOrDefault();

            if (result != null)
            {
                return result.PassRate;
            }

            return 0;
        }

        public Course GetCourseByKiosk(int siteCode)
        {
            var result = _appdDbContext.Courses
                .Include(s => s.Course_Questions)
                  .Where(s => s.FK_SiteCode == siteCode)
                  .FirstOrDefault();           

            return result;
        }

        public List<ExamQuestionModel> GetCourseQuestionsBySite(int siteCode, string lang)
        {
            var results = _appdDbContext.Course_Questions
                .Include(s => s.Question.Answers)
                .Include(s => s.Course)
                .Where(s => s.Course.FK_SiteCode == siteCode && s.Course.IsActive == true)
                .Select(s => new ExamQuestionModel
                {
                    Code = s.Question.Code,
                    Question = lang == "si" ? s.Question.TextSN : lang == "ta" ? s.Question.TextTA : s.Question.TextEN,
                    Answers = s.Question.Answers.Select(a => new ExamAnswerModel
                    {
                        Code = a.Code,
                        Answer = lang == "si" ? a.TextSN : lang == "ta" ? a.TextTA : a.TextEN
                    }).ToList()
                }).ToList();

            return results;
        }

        public Course GetCoursBySite(int siteCode)
        {
            var result = (from d in _appdDbContext.Courses
                           where d.FK_SiteCode == siteCode && d.IsActive == true
                           select d).SingleOrDefault();

            return result;
        }

        public bool IsQuestionAnswerCorrect(int questionCode, int answerCode)
        {
            var results = _appdDbContext.Answers
                .Where(s => s.IsCorrect && s.FK_QuestionCode == questionCode && s.Code == answerCode).Any();

            return results;
        }

        public List<DisplayCourseModel> GetAll()
        {
            var results = _appdDbContext.Courses
                .Include(s=>s.Site)
                         .Select(s => new DisplayCourseModel
                         {
                             Code = s.Code,
                             Video = s.Video,
                             PassRate = s.PassRate,
                             IsActive = s.IsActive,
                             Title = s.Title,
                             Location = s.Site.Location,
                             SiteName = s.Site.Name
                         }).ToList();

            return results;
        }

        public Course GetCourseByID(int code)
        {
            var result = _appdDbContext.Courses.SingleOrDefault(s=>s.Code == code);
            return result;
        }

        public List<QuestionModel> GetQuestionByCourseID(int courseId)
        {
            var results = _appdDbContext.Course_Questions
                          .Include(s => s.Question.Answers)
                          .Where(s=>s.FK_CourseCode == courseId && s.Question.IsActive)
                         .Select(s => new QuestionModel
                         {
                             Code = s.Question.Code,
                             TextEN = s.Question.TextEN,
                             TextSN = s.Question.TextSN,
                             TextTA = s.Question.TextTA,
                             IsActive = s.Question.IsActive
                         }).ToList();

            return results;
        }

        public void UpdateQuestionsByCourse(int courseId, List<int> questions)
        {
            //TODO: Need to add User ID

            var courseQuestions = _appdDbContext.Course_Questions
                          .Where(s => s.FK_CourseCode == courseId);

            if (courseQuestions.Any())
            {
                _appdDbContext.Course_Questions.RemoveRange(courseQuestions);
                _appdDbContext.SaveChanges();
            }

            foreach (var item in questions)
            {
                _appdDbContext.Course_Questions.Add(new Course_Question
                {
                    FK_CourseCode = courseId,
                    FK_QuestionCode = item,
                    //ModifiedBy = "1be19cad-70ef-4758-a6fc-3460bdcfdeb4",
                    ModifiedDateTime = DateTime.Now
                });
            }

            _appdDbContext.SaveChanges();
        }
    }
}


