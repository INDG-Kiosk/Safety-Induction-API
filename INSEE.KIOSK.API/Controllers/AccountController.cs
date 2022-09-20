using INSEE.KIOSK.API.Model;
using INSEE.KIOSK.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Controllers
{
      //  [Route("api/[controller]")]
        [ApiController]
        public class AccountController : ControllerBase
        {
        private const string API_ROUTE_NAME = "api/accounts";
        private readonly IUserService _userService;
            public AccountController(IUserService userService)
            {
                _userService = userService;
            }
            [HttpPost("register")]
            //[ApiExplorerSettings(IgnoreApi = true)]
            public async Task<ActionResult> RegisterAsync(InsertAccount model)
            {

                var result = await _userService.RegisterAsync(model);
                return Ok(result);
            }

        [HttpGet]
        [Route(API_ROUTE_NAME + "/loggeduser")]
        public async Task<IActionResult> GetLoggedUserDetails()
        {
            try
            {
                var user = _userService.GetById(User.Identities.First().Claims.Single(s => s.Type == "uid").Value);
                var roles = await _userService.GetRolesById(User.Identities.First().Claims.Single(s => s.Type == "uid").Value);
                var userModel = new EditAccount()
                {
                    Code = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LockoutEnabled = (DateTime.Now < user.LockoutEnd) ? true : false,
                    Role = roles.FirstOrDefault()
                };
                return Ok(new Message<EditAccount> { Status = "S", Result = userModel });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPost]
        [Route(API_ROUTE_NAME + "/token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            if (result.IsAuthenticated)
            {
                SetRefreshTokenInCookie(result.RefreshToken);
            }
            return Ok(result);
        }

            [HttpPost("addrole")]
            public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
            {
                var result = await _userService.AddRoleAsync(model);
                return Ok(result);
            }
            [HttpPost("refresh-token")]
            public async Task<IActionResult> RefreshToken()
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var response = await _userService.RefreshTokenAsync(refreshToken);
                if (!string.IsNullOrEmpty(response.RefreshToken))
                    SetRefreshTokenInCookie(response.RefreshToken);
                return Ok(response);
            }


            [HttpPost("revoke-token")]
            public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { message = "Token is required" });

                var response = _userService.RevokeToken(token);

                if (!response)
                    return NotFound(new { message = "Token not found" });

                return Ok(new { message = "Token revoked" });
            }
            private void SetRefreshTokenInCookie(string refreshToken)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(10),
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            }

            [Authorize]
            [HttpPost("tokens/{id}")]
            public IActionResult GetRefreshTokens(string id)
            {
                var user = _userService.GetById(id);
                return Ok(user.RefreshTokens);
            }

        [HttpGet]
        [Route(API_ROUTE_NAME)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(new Message<List<Account>> { Status = "S", Result = _userService.GetAll() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }


        [HttpGet]
        [Route(API_ROUTE_NAME + "/{id}")]
        public async Task<IActionResult> GetAccountDetailByID(string id)
        {
            try
            {
                var user = _userService.GetById(id);
                var roles = await _userService.GetRolesById(id);
                var userModel = new EditAccount()
                {
                    Code = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LockoutEnabled = (DateTime.Now < user.LockoutEnd)?true:false,
                    Role = roles.FirstOrDefault()
                };
                return Ok(new Message<EditAccount> { Status = "S", Result = userModel });
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPost]
        [Route(API_ROUTE_NAME)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Insert([FromBody] Model.InsertAccount model)
        {
            try
            {
                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("Id").Value;

                //TODO: COMMENT BEFORE GO LIVE
                //TODO: Need to add User Id
                return Ok(await _userService.RegisterAsync(new InsertAccount()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Role = model.Role,
                    ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                    ModifiedDateTime = DateTime.Now
                }));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }

        [HttpPut]
        [Route(API_ROUTE_NAME)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update([FromBody] Model.EditAccount model)
        {
            try
            {
                model.ModifiedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value;
                model.ModifiedDateTime = DateTime.Now;
                return Ok(await _userService.UpdateAsync(model));

            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.BadRequest, new Message<string>() { Text = ex.Message });
            }
        }
    }
    }
