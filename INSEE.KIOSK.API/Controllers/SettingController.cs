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

    [ApiController]
    public class SettingController : Controller
    {
        readonly Services.ISettingService _settingService;
        readonly Services.ICompanyService _companyService;
        List<ValidationResult> validations;
        public SettingController(Services.ISettingService settingService, Services.ICompanyService companyService)
        {
            _settingService = settingService;
            _companyService = companyService;
            validations = new List<ValidationResult>();
        }

        [HttpGet]
        [Route("api/setting")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetSetting()
        {
            try
            {
                var settings = _settingService.GetSetting();
                return Ok(new Message<Model.SettingModel>()
                {
                    Status = "S",
                    Result = new SettingModel()
                    {
                        Code = settings.Code,
                        PassValidPeridINMonthsForVisitor = settings.PassValidPeridINMonthsForVisitor,
                        PassValidPeridINMonthsForWorker = settings.PassValidPeridINMonthsForWorker,
                        ReprintValidDaysForWorker = settings.ReprintValidDaysForWorker,
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/setting")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Insert([FromBody] Model.SettingModel model)
        {
            return Ok(_settingService.Insert(new Context.Settings()
            {
                FK_CompanyCode = _companyService.GetDefualtCompany().Code,
                PassValidPeridINMonthsForVisitor = model.PassValidPeridINMonthsForVisitor,
                PassValidPeridINMonthsForWorker = model.PassValidPeridINMonthsForWorker,
                ReprintValidDaysForWorker = model.ReprintValidDaysForWorker,
                ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                Modified_DateTime = DateTime.Now,
            }));
        }

        [HttpPut]
        [Route("api/setting")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update([FromBody] Model.SettingModel model)
        {
            var result = _settingService.Update(new Context.Settings()
            {
                PassValidPeridINMonthsForVisitor = model.PassValidPeridINMonthsForVisitor,
                PassValidPeridINMonthsForWorker = model.PassValidPeridINMonthsForWorker,
                ReprintValidDaysForWorker = model.ReprintValidDaysForWorker,
                ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                Modified_DateTime = DateTime.Now,
            });
            return Ok(result);

        }
    }
}
