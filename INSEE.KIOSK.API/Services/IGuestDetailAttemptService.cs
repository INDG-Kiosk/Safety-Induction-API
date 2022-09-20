using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface IGuestDetailAttemptService
    {
        public Message<string> Insert(Guest_Detail_Attempt visitorDetailAttempt);
        public Message<string> Update(Guest_Detail_Attempt visitorDetailAttempt);
        public Guest_Detail_Attempt GetLastCompletedAttempt(string nic);
        public Guest_Detail_Attempt GetLastStartedAttempt(string nic);
        public List<VW_DailyGuestSummary> GetAllLastAttempts(DateTime fromDate, DateTime toDate);
        public List<VW_GuestSummary> GetAttemptDetailsByNIC(string nic);
        public int TotalRegistered();
    }

    public class GuestDetailAttemptService : IGuestDetailAttemptService
    {
        readonly ApplicationDbContext _appdDbContext;
        public GuestDetailAttemptService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public Message<string> Insert(Guest_Detail_Attempt visitorDetailAttempt)
        {
            _appdDbContext.Guest_Detail_Attempts.Add(visitorDetailAttempt);
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"New Visitor Attempt Registered", Status = "S" };
        }

        public Message<string> Update(Guest_Detail_Attempt visitorDetailAttempt)
        {
            var result = _appdDbContext.Guest_Detail_Attempts.SingleOrDefault(s => s.Code == visitorDetailAttempt.Code);
            if (result == null)
            {
                return new Message<string>() { Text = $"Guest Attempt Not Found" };
            }

            result.Print_Count = visitorDetailAttempt.Print_Count;
            result.Status = visitorDetailAttempt.Status;
            result.TestCompletedDateTime = visitorDetailAttempt.TestCompletedDateTime;
            result.TestStartedDateTime = visitorDetailAttempt.TestStartedDateTime;
            result.TotalMarks = visitorDetailAttempt.TotalMarks;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"Attempt Successfully Updated", Status = "S" };
        }

        public List<VW_DailyGuestSummary> GetAllLastAttempts(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = (from s in _appdDbContext.VW_DailyGuestSummary
                              where s.inserted.Date >= fromDate.Date && s.inserted.Date <= toDate.Date
                              orderby s.inserted descending
                              select s).ToList();
                return result;
            }catch (Exception ex)
            {
                return null;
            }
        }

        public int TotalRegistered()
        {
            try
            {
                var result = (from s in _appdDbContext.VW_DailyGuestSummary
                              where ((s.Reason == "WORKER" && s.ExamStatus == "PASSED") || s.Reason == "VISITOR")
                              select s.PersonNIC).Count();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<VW_GuestSummary> GetAttemptDetailsByNIC (string nic)
        {
            try
            {
                var result = (from s in _appdDbContext.VW_GuestSummary
                              where s.PersonNIC.ToLower().Equals(nic.ToLower())
                              orderby s.inserted descending
                              select s).ToList();

                return result;
            }catch (Exception ex)
            {
                return null;
            }
        }   


        public Guest_Detail_Attempt GetLastCompletedAttempt(string nic)
        {
            var result = _appdDbContext.Guest_Detail_Attempts
                .Include(s => s.Guest_Detail)
                .OrderByDescending(s=>s.TestCompletedDateTime)
                .LastOrDefault(s => s.Guest_Detail.Guest_Master.NIC.ToLower() == nic.ToLower());                

            return result;
        }

        public Guest_Detail_Attempt GetLastStartedAttempt(string nic)
        {
            var result = _appdDbContext.Guest_Detail_Attempts
                .Include(s => s.Guest_Detail)
                .OrderByDescending(s => s.TestStartedDateTime)
                .FirstOrDefault(s => s.Guest_Detail.Guest_Master.NIC.ToLower() == nic.ToLower());


            return result;
        }
    }
}


