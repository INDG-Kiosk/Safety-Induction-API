using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface ICompanyService 
    {
        public Message<string> Insert(Company company);
        public Message<string> Update(Company company);
        public Message<Company> GetCompanyByID(int code);

        public Company GetDefualtCompany();
        public void Initial();
    }

    public class CompanyService : ICompanyService
    {
        readonly ApplicationDbContext _appdDbContext;
        public CompanyService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public void Initial()
        {
            if (_appdDbContext.Companies.FirstOrDefault() == null)
            {
                _appdDbContext.Companies.Add(new Company
                {
                    CountryCode = "LK",
                    IsActive = true,
                    ModifiedDateTime = DateTime.Now,
                    Name = "INSEE",
                    ModifiedBy = _appdDbContext.Users.First().Id,
                    Sites = new List<Site>() { new Site() {
                            IP = "1.1.1.1",
                            IsActive = true,
                            ModifiedBy = _appdDbContext.Users.First().Id,
                            Location = "PUTHALAM",
                            ModifiedDateTime = DateTime.Now,
                            ResourcePath = "",
                             Courses = new List<Course>()
                            {
                                new Course()
                                {
                                    Title = "Course1",
                                    IsActive = true,
                                    PassRate = 80,
                                    ModifiedBy = _appdDbContext.Users.First().Id,
                                    ModifiedDateTime = DateTime.Now,
                                    Course_Questions = new List<Course_Question>(){
                                    new Course_Question(){
                                    Question = new Question()
                                    {
                                        IsActive = true,
                                        ModifiedBy = _appdDbContext.Users.First().Id,
                                        ModifiedDateTime = DateTime.Now,
                                        TextEN = "Q1",
                                        TextSN = "Q1",
                                        TextTA = "Q1",
                                        Answers = new List<Answer>() {
                                        new Answer()
                                        {
                                            TextEN = "A1",
                                            TextSN = "A1",
                                            TextTA = "A1",
                                            IsCorrect = true,
                                        },
                                         new Answer()
                                        {
                                            TextEN = "A2",
                                            TextSN = "A2",
                                            TextTA = "A2"
                                        },
                                          new Answer()
                                        {
                                            TextEN = "A3",
                                            TextSN = "A3",
                                            TextTA = "A3"
                                        },
                                           new Answer()
                                        {
                                            TextEN = "A4",
                                            TextSN = "A4",
                                            TextTA = "A4"
                                        }
                                        },

                                    },
                                    ModifiedBy = _appdDbContext.Users.First().Id,
                                    ModifiedDateTime = DateTime.Now
                                    },
                                    new Course_Question(){
                                    Question = new Question()
                                    {
                                        IsActive = true,
                                        ModifiedBy = _appdDbContext.Users.First().Id,
                                        ModifiedDateTime = DateTime.Now,
                                        TextEN = "Q2",
                                        TextSN = "Q2",
                                        TextTA = "Q2",
                                        Answers = new List<Answer>() {
                                        new Answer()
                                        {
                                            TextEN = "A1",
                                            TextSN = "A1",
                                            TextTA = "A1",
                                            IsCorrect = true,
                                        },
                                         new Answer()
                                        {
                                            TextEN = "A2",
                                            TextSN = "A2",
                                            TextTA = "A2"
                                        },
                                          new Answer()
                                        {
                                            TextEN = "A3",
                                            TextSN = "A3",
                                            TextTA = "A3"
                                        },
                                           new Answer()
                                        {
                                            TextEN = "A4",
                                            TextSN = "A4",
                                            TextTA = "A4"
                                        }
                                        },

                                    },
                                    ModifiedBy = _appdDbContext.Users.First().Id,
                                    ModifiedDateTime = DateTime.Now
                                    },
                                    new Course_Question(){
                                    Question = new Question()
                                    {
                                        IsActive = true,
                                        ModifiedBy = _appdDbContext.Users.First().Id,
                                        ModifiedDateTime = DateTime.Now,
                                        TextEN = "Q3",
                                        TextSN = "Q3",
                                        TextTA = "Q3",
                                        Answers = new List<Answer>() {
                                        new Answer()
                                        {
                                            TextEN = "A1",
                                            TextSN = "A1",
                                            TextTA = "A1",
                                            IsCorrect = true,
                                        },
                                         new Answer()
                                        {
                                            TextEN = "A2",
                                            TextSN = "A2",
                                            TextTA = "A2"
                                        },
                                          new Answer()
                                        {
                                            TextEN = "A3",
                                            TextSN = "A3",
                                            TextTA = "A3"
                                        },
                                           new Answer()
                                        {
                                            TextEN = "A4",
                                            TextSN = "A4",
                                            TextTA = "A4"
                                        }
                                        },

                                    },
                                    ModifiedBy = _appdDbContext.Users.First().Id,
                                    ModifiedDateTime = DateTime.Now
                                    },new Course_Question(){
                                    Question = new Question()
                                    {
                                        IsActive = true,
                                        ModifiedBy = _appdDbContext.Users.First().Id,
                                        ModifiedDateTime = DateTime.Now,
                                        TextEN = "Q4",
                                        TextSN = "4",
                                        TextTA = "Q4",
                                        Answers = new List<Answer>() {
                                        new Answer()
                                        {
                                            TextEN = "A1",
                                            TextSN = "A1",
                                            TextTA = "A1",
                                            IsCorrect = true
                                        },
                                         new Answer()
                                        {
                                            TextEN = "A2",
                                            TextSN = "A2",
                                            TextTA = "A2"
                                        },
                                          new Answer()
                                        {
                                            TextEN = "A3",
                                            TextSN = "A3",
                                            TextTA = "A3"
                                        },
                                           new Answer()
                                        {
                                            TextEN = "A4",
                                            TextSN = "A4",
                                            TextTA = "A4"
                                        }
                                        },

                                    },
                                    ModifiedBy = _appdDbContext.Users.First().Id,
                                    ModifiedDateTime = DateTime.Now
                                    },new Course_Question(){
                                    Question = new Question()
                                    {
                                        IsActive = true,
                                        ModifiedBy = _appdDbContext.Users.First().Id,
                                        ModifiedDateTime = DateTime.Now,
                                        TextEN = "Q5",
                                        TextSN = "Q5",
                                        TextTA = "Q5",
                                        Answers = new List<Answer>() {
                                        new Answer()
                                        {
                                            TextEN = "A1",
                                            TextSN = "A1",
                                            TextTA = "A1",
                                            IsCorrect = true
                                        },
                                         new Answer()
                                        {
                                            TextEN = "A2",
                                            TextSN = "A2",
                                            TextTA = "A2"
                                        },
                                          new Answer()
                                        {
                                            TextEN = "A3",
                                            TextSN = "A3",
                                            TextTA = "A3"
                                        },
                                           new Answer()
                                        {
                                            TextEN = "A4",
                                            TextSN = "A4",
                                            TextTA = "A4"
                                        }
                                        },

                                    },
                                    ModifiedBy = _appdDbContext.Users.First().Id,
                                    ModifiedDateTime = DateTime.Now
                                    }

                                }
                            }
                            }
                    }
                    },
                    //Settings = new Settings()
                    //{
                    //    FK_CompanyCode = _appdDbContext.Companies.First().Code,
                    //    ModifiedBy = _appdDbContext.Users.First().Id,
                    //    Modified_DateTime = DateTime.Now,
                    //    PassValidPeridINMonthsForVisitor = 10,
                    //    PassValidPeridINMonthsForWorker = 10,
                    //    ReprintValidDaysForWorker = 10,
                    //},
                    //Contractors_Master = new List<Contractor_Master>()
                    //{
                    //    new Contractor_Master()
                    //    {
                    //         ModifiedBy = _appdDbContext.Users.First().Id,
                    //ModifiedDateTime = DateTime.Now,
                    //IsActive = true,
                    //NameEN = "A1",
                    //NameSN = "A1S",
                    //NameTA = "A1T"
                    //    },
                    //    new Contractor_Master()
                    //    {
                    //         ModifiedBy = _appdDbContext.Users.First().Id,
                    //ModifiedDateTime = DateTime.Now,
                    //IsActive = true,
                    //NameEN = "A2",
                    //NameSN = "A2S",
                    //NameTA = "A2T"
                    //    },new Contractor_Master()
                    //    {
                    //         ModifiedBy = _appdDbContext.Users.First().Id,
                    //ModifiedDateTime = DateTime.Now,
                    //IsActive = true,
                    //NameEN = "A2",
                    //NameSN = "A2S",
                    //NameTA = "A2T"
                    //    }
                    //}
                });
                _appdDbContext.SaveChanges();
            }

            if (_appdDbContext.Settings.Count() == 0)
            {
                _appdDbContext.Settings.Add(new Settings()
                {
                    FK_CompanyCode = _appdDbContext.Companies.First().Code,
                    ModifiedBy = _appdDbContext.Users.First().Id,
                    Modified_DateTime = DateTime.Now,
                    PassValidPeridINMonthsForVisitor = 10,
                    PassValidPeridINMonthsForWorker = 10,
                    ReprintValidDaysForWorker = 10,
                });
                _appdDbContext.SaveChanges();
            }


            if (_appdDbContext.Contractors_Master.Count() == 0)
            {
                _appdDbContext.Contractors_Master.Add(new Contractor_Master()
                {
                    FK_CompanyCode = _appdDbContext.Companies.First().Code,
                    ModifiedBy = _appdDbContext.Users.First().Id,
                    ModifiedDateTime = DateTime.Now,
                    IsActive = true,
                    NameEN = "A1",
                    NameSN = "A1S",
                    NameTA = "A1T"
                });
                _appdDbContext.Contractors_Master.Add(new Contractor_Master()
                {
                    FK_CompanyCode = _appdDbContext.Companies.First().Code,
                    ModifiedBy = _appdDbContext.Users.First().Id,
                    ModifiedDateTime = DateTime.Now,
                    IsActive = true,
                    NameEN = "A2",
                    NameSN = "A2S",
                    NameTA = "A2T"
                });
                _appdDbContext.Contractors_Master.Add(new Contractor_Master()
                {
                    FK_CompanyCode = _appdDbContext.Companies.First().Code,
                    ModifiedBy = _appdDbContext.Users.First().Id,
                    ModifiedDateTime = DateTime.Now,
                    IsActive = true,
                    NameEN = "A3",
                    NameSN = "A3S",
                    NameTA = "A3T"
                });
                _appdDbContext.SaveChanges();
            }
        }
        public Message<Company> GetCompanyByID(int code)
        {
            var result = _appdDbContext.Companies.Include(o => o.User).SingleOrDefault(s=>s.Code == code);
            return new Message<Company>() { Result = result, Status = "S" };
        }

        public Message<string> Insert(Company company)
        {
            _appdDbContext.Companies.Add(company);
            _appdDbContext.SaveChanges();
            return new Message<string>() { Text = "Company Successfully Added", Status = "S" };
        }

        public Message<string> Update(Company company)
        {
            var result = _appdDbContext.Companies.SingleOrDefault(s => s.Code == company.Code);
            if(result == null)
            {
                return new Message<string>() { Text = "Company Not Found" };
            }

            result.Name = company.Name;
            result.IsActive = company.IsActive;
            //result.Modified_By = company.Modified_By;
            result.ModifiedDateTime = company.ModifiedDateTime;
            result.CountryCode = company.CountryCode;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = "Company Successfully Updated", Status = "S" };
        }

        public Company GetDefualtCompany()
        {
            var result = _appdDbContext.Companies.FirstOrDefault();
            return result;
        }
    }
}
