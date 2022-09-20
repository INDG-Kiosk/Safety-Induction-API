using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface IGuestDetailService
    {
        public Message<string> Insert(Guest_Detail guestDetail);
        public Message<string> Update(Guest_Detail guestDetail);
    }

    public class GuestDetailService : IGuestDetailService
    {
        readonly ApplicationDbContext _appdDbContext;
        public GuestDetailService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }      
        
        public Message<string> Insert(Guest_Detail guestDetail)
        {
            _appdDbContext.Guest_Details.Add(guestDetail);
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"New Guest Detail Registered", Status = "S" };
        }        

        public Message<string> Update(Guest_Detail guestDetail)
        {
            var result = _appdDbContext.Guest_Details.SingleOrDefault(s => s.Code == guestDetail.Code);
            if (result == null)
            {
                return new Message<string>() { Text = $"Guest Detail Not Found" };
            }

            result.Contractor_Master = guestDetail.Contractor_Master;
            result.ProfileImg = guestDetail.ProfileImg;
            result.Type = guestDetail.Type;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"Guest Detail Successfully Updated", Status = "S" };
        }        
    }
}


