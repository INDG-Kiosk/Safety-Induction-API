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
    //[Authorize(Roles = "Administrator")]
    [ApiController]
    public class ContractorController : Controller
    {
        private const string API_ROUTE_NAME = "api/contractors";

        readonly Services.IContractorMasterService _contractorMasterService;
        readonly Services.ICompanyService _companyService;
        List<ValidationResult> validations;
        public ContractorController(Services.IContractorMasterService contractorMasterService, Services.ICompanyService companyService)
        {
            _contractorMasterService = contractorMasterService;           
            _companyService = companyService;
            validations = new List<ValidationResult>();
        }

        [HttpGet]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contractors = _contractorMasterService.GetAll();

                return Ok(new Message<List<ContractorModel>>()
                {
                    Status = "S",
                    Result = contractors
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpGet]
        [Route(API_ROUTE_NAME + "/{id}")]
        public async Task<IActionResult> GetContractorByID(int id)
        {
            if (id == 0)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Your request is missing parameters" });
            }

            try
            {
                var contractorMaster = _contractorMasterService.GetContractorByID(id);

                if (contractorMaster == null)               
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Invalid Contractor ID" });                

                var result = new Model.ContractorModel()
                {
                    Code = contractorMaster.Code,
                    NameEN = contractorMaster.NameEN,
                    NameSN = contractorMaster.NameSN,
                    NameTA = contractorMaster.NameTA,
                    MailingAddress = contractorMaster.MailingAddress,
                    IsActive = contractorMaster.IsActive
                };

                return Ok(new Message<ContractorModel>()
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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route(API_ROUTE_NAME)]
        //  [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Insert([FromBody] Model.InsertContractorModel model)
        {
            try
            {
                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("Id").Value;

                var company = _companyService.GetDefualtCompany();

                if (company == null)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Company is missing!" });
                }

                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add USER ID
                return Ok(_contractorMasterService.Insert(new Contractor_Master()
                {
                    NameEN = model.NameEN,
                    NameSN = model.NameSN,
                    NameTA = model.NameTA,
                    MailingAddress = model.MailingAddress,
                    FK_CompanyCode = company.Code,
                    IsActive = true,
                    ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                    ModifiedDateTime = DateTime.Now
                }));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route(API_ROUTE_NAME)]
        public async Task<IActionResult> Update([FromBody] Model.UpdateContractorModel model)
        {
            try
            {
                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User ID

                var contractor = _contractorMasterService.GetContractorByID(model.Code);

                if (contractor == null)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Invalid Contractor ID" });

                contractor.NameEN = model.NameEN;
                contractor.NameSN = model.NameSN;
                contractor.NameTA = model.NameTA;
                contractor.MailingAddress = model.MailingAddress;
                contractor.ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
                contractor.ModifiedDateTime = DateTime.Now;
                contractor.IsActive = model.IsActive;

                var result = _contractorMasterService.Update(contractor);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }
    }
}
