using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface IGuestMasterService
    {
        public Message<string> Insert(Guest_Master guest_Master);
        public Message<string> Update(Guest_Master guest_Master);
        public bool IsGuestExist(string nic);
        public Guest_Master GetGuestByNIC(string nic);
    }

    public class GuestMasterService : IGuestMasterService
    {
        readonly ApplicationDbContext _appdDbContext;
        public GuestMasterService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public Message<string> Insert(Guest_Master guest_Master)
        {
            _appdDbContext.Guest_Master.Add(guest_Master);
            _appdDbContext.SaveChanges();
            return new Message<string>() { Text = $"New Guest Master {guest_Master.NIC} Registered", Status = "S" };
        }

        public Message<string> Update(Guest_Master guest_Master)
        {
            var result = _appdDbContext.Guest_Master.SingleOrDefault(s => s.Code == guest_Master.Code);
            if (result == null)
            {
                return new Message<string>() { Text = $"Guest master {guest_Master.NIC} Not Found" };
            }

            result.Name = guest_Master.Name;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"Guest master { guest_Master.NIC } Successfully Updated", Status = "S" };
        }

        public bool IsGuestExist(string nic)
        {
            var result = _appdDbContext.Guest_Master.FirstOrDefault(s => s.NIC.ToLower() == nic.ToLower());

            if (result != null)
            {
                return true;
            }
            return false;
        }

        public Guest_Master GetGuestByNIC(string nic)
        {
            var result = _appdDbContext.Guest_Master.FirstOrDefault(s => s.NIC.ToLower() == nic.ToLower());
            return result;
        }
    }
}
