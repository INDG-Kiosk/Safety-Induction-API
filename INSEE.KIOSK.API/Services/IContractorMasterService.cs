using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface IContractorMasterService
    {
        public Message<string> Insert(Contractor_Master contractor_Master);
        public Message<string> Update(Contractor_Master contractor_Master);
        public List<ContractorModel> GetAll();
        public Contractor_Master GetContractorByID(int code);
    }

    public class ContractorMasterService : IContractorMasterService
    {
        readonly ApplicationDbContext _appdDbContext;
        public ContractorMasterService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public Message<string> Insert(Contractor_Master contractor_Master)
        {
            _appdDbContext.Contractors_Master.Add(contractor_Master);
            _appdDbContext.SaveChanges();
            //TODO: ASK Poora, add by gevan id sent to the client
            return new Message<string>() { Text = $"New Contractor Master {contractor_Master.NameEN} Registered", Status = "S", Result = contractor_Master.Code.ToString()
            };
        }

        public Message<string> Update(Contractor_Master contractor_Master)
        {
            var result = _appdDbContext.Contractors_Master.SingleOrDefault(s => s.Code == contractor_Master.Code);
            if (result == null)
            {
                return new Message<string>() { Text = $"Contractor Master {contractor_Master.NameEN} Not Found" };
            }

            result.NameEN = contractor_Master.NameEN;
            result.NameSN = contractor_Master.NameSN;
            result.NameTA = contractor_Master.NameTA;
            result.ModifiedBy = contractor_Master.ModifiedBy;
            result.ModifiedDateTime = DateTime.Now;
            result.MailingAddress = contractor_Master.MailingAddress;
            result.IsActive = contractor_Master.IsActive;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"Contractor master { contractor_Master.NameEN } Successfully Updated", Status = "S" };
        }

        public List<ContractorModel> GetAll()
        {
            var results = _appdDbContext.Contractors_Master
                .Select(s => new ContractorModel
                {
                    Code = s.Code,
                    NameEN = s.NameEN,
                    NameSN = s.NameSN, 
                    NameTA = s.NameTA,
                    IsActive = s.IsActive,
                    MailingAddress = s.MailingAddress,

                }).ToList();

            return results;
        }

        public Contractor_Master GetContractorByID(int code)
        {
            var result = _appdDbContext.Contractors_Master.SingleOrDefault(s => s.Code == code);
            return result;
        }
    }
}
