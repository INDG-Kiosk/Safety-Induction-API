using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    #region Questions - Master Data
    public class QuestionModel
    {
        public virtual int Code { get; set; }

        public virtual string TextEN { get; set; } = string.Empty;
        public virtual string TextSN { get; set; } = string.Empty;
        public virtual string TextTA { get; set; } = string.Empty;

        public virtual bool IsActive { get; set; } = false;
        public List<AnswerModel> Answers { get; set; } = new List<AnswerModel>();
    }

    public class InsertQuestionModel : QuestionModel
    {
        [Required(ErrorMessage = "Question English Name is required")]
        public override string TextEN { get; set; }

        [Required(ErrorMessage = "Question Sinhala Name is required")]
        public override string TextSN { get; set; }

        [Required(ErrorMessage = "Question Tamil Name is required")]
        public override string TextTA { get; set; }

        public override bool IsActive { get; set; } = false;
        public List<InsertAnswerModel> Answers { get; set; } = new List<InsertAnswerModel>();
    }

    public class UpdateQuestionModel : QuestionModel
    {
        [Required(ErrorMessage = "Question Code is required")]
        public override int Code { get; set; }

        [Required(ErrorMessage = "Question English Name is required")]
        public override string TextEN { get; set; }

        [Required(ErrorMessage = "Question Sinhala Name is required")]
        public override string TextSN { get; set; }

        [Required(ErrorMessage = "Question Tamil Name is required")]
        public override string TextTA { get; set; }
     
        public override bool IsActive { get; set; } = false;

        public string LastUpdated { get; set; }

        public List<UpdateAnswerModel> Answers { get; set; } = new List<UpdateAnswerModel>();
    }

    #endregion

    #region Answers - Master Data

    public class AnswerModel
    {
        public virtual int Code { get; set; }

        public virtual string TextEN { get; set; } = string.Empty;
        public virtual string TextSN { get; set; } = string.Empty;
        public virtual string TextTA { get; set; } = string.Empty;

        public virtual bool IsActive { get; set; } = false;
        public virtual bool IsCorrectAnswer { get; set; } = false;
    }

    public class InsertAnswerModel : AnswerModel
    {
        [Required(ErrorMessage = "Answer English Name is required")]
        public override string TextEN { get; set; }

        [Required(ErrorMessage = "Answer Sinhala Name is required")]
        public override string TextSN { get; set; }

        [Required(ErrorMessage = "Answer Tamil Name is required")]
        public override string TextTA { get; set; }

        public override bool IsCorrectAnswer { get; set; } = false;

        public override bool IsActive { get; set; }
    }

    public class UpdateAnswerModel : AnswerModel
    {
        [Required(ErrorMessage = "Answer Code is required")]
        public override int Code { get; set; }

        [Required(ErrorMessage = "Answer English Name is required")]
        public override string TextEN { get; set; }

        [Required(ErrorMessage = "Answer Sinhala Name is required")]
        public override string TextSN { get; set; }

        [Required(ErrorMessage = "Answer Tamil Name is required")]
        public override string TextTA { get; set; }

        public override bool IsCorrectAnswer { get; set; } = false;

        public override bool IsActive { get; set; }
    }

    #endregion

    #region Exam Questions & Answers

    public class ExamQuestionModel
    {
        public int Code { get; set; }

        public string Question { get; set; } = string.Empty;
        public List<ExamAnswerModel> Answers { get; set; } = new List<ExamAnswerModel>();
    }

    public class ExamAnswerModel
    {
        public int Code { get; set; }

        public string Answer { get; set; } = string.Empty;

    }

    public class ExamQuestionAnswerPostModel
    {
        [Required(ErrorMessage = "NIC is required")]
        public string NIC { get; set; } = string.Empty;
        public List<ExamQuestionAnswerModel> QuestionAnswers { get; set; } = new List<ExamQuestionAnswerModel>();
    }

    public class ExamQuestionAnswerModel
    {
        public int Question { get; set; } = 0;

        public int Answer { get; set; } = 0;
    }

    #endregion
}
