using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Controllers
{
    // [Authorize]
     [Authorize(Roles = "Administrator")]
    [ApiController]
    public class CompanyController : Controller
    {
        readonly Services.ICompanyService _companyService;
        readonly Services.ICourseService _courseService;
        List<ValidationResult> validations;
        public CompanyController(Services.ICompanyService companyService, Services.ICourseService courseService)
        {
            _companyService = companyService;
            _courseService = courseService;
            validations = new List<ValidationResult>();
        }

        [HttpGet]
        [Route("api/company/{id}")]
        public async Task<IActionResult> GetCompanyByID(int id = 0)
        {
            //if (id == 0)
            //{
            //    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Your request is missing parameters" });
            //}

            try
            {
                var company = _companyService.GetDefualtCompany();
                if (company == null)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = $"Invalid Company ID" });
                }

                var result = new Model.CompanyModel()
                {
                    Code = company.Code,
                    Country_Code = company.CountryCode,
                    IsActive = company.IsActive,
                    Last_Modified_By = company.User.FirstName,
                    Last_Modified_Date = company.ModifiedDateTime.ToString(CommonResources.default_dateformat),
                    Name = company.Name
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }
        
        [HttpPost]
        [Route("api/company/add")]
        //  [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Insert([FromBody] Model.CompanyModel model)
        {
            //TODO: COMMENT BEFORE GO LIVE
            //TODO: NEED to discussed with poora about the return & api path
            return Ok(_companyService.Insert(new Company()
            {
                CountryCode = model.Country_Code,
                ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                ModifiedDateTime = DateTime.Now,
                IsActive = true,
                Name = model.Name,
            }))  ;
        }

        [HttpPost]
        [Route("api/company/update")]
        public async Task<IActionResult> Update([FromBody] Model.UpdateCompanyModel model)
        {
            //TODO: NEED to discussed with poora about the return & api path
            //if (!CommonResources.Validate(model, ref validations))
            //{
            //    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = String.Join(",", validations) });
            //}

            var dbCompany = _companyService.GetDefualtCompany();
            if (dbCompany == null)
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = "Invalid Company ID" });

            dbCompany.Name = model.Name;
            //TODO: COMMENT BEFORE GO LIVE
            dbCompany.ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
            dbCompany.ModifiedDateTime = DateTime.Now;
            dbCompany.IsActive = model.IsActive;
            dbCompany.CountryCode = model.Country_Code;

            var result = _companyService.Update(dbCompany);
            return Ok(result);

        }
    }
}
