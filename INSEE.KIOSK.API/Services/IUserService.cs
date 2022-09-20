using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface IUserService
    {
        Task<Message<string>> RegisterAsync(InsertAccount model);
        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<AuthenticationModel> RefreshTokenAsync(string jwtToken);
        bool RevokeToken(string token);
        ApplicationUser GetById(string id);
        Task<List<string>> GetRolesById(string id);

        public List<Account> GetAll();
        Task<Message<string>> UpdateAsync(EditAccount model);

    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public const string AD_Server_IP = "inseegroup.com";

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public List<Account> GetAll()
        {
            return (from a in _context.Users
                    select new Account()
                    {
                        Code = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        LockoutEnabled = (DateTime.Now < a.LockoutEnd) ? true : false,
                    }).ToList();
        }



        public async Task<Message<string>> UpdateAsync(EditAccount model)
        {
            var existUser = GetById(model.Code);
            var roles = await _userManager.GetRolesAsync(existUser);

            existUser.FirstName = model.FirstName;
            existUser.LastName = model.LastName;
            existUser.PhoneNumber = model.PhoneNumber;
            existUser.LockoutEnd = (model.LockoutEnabled) ? new DateTime(9999, 12, 31) : DateTime.Now.AddDays(-1);
            //existUser.LastUpdatedBy = model.ModifiedBy;
            // existUser.LastUpdatedDateTime = model.ModifiedDateTime;


            try
            {
                var result = await _userManager.UpdateAsync(existUser);
                if (result.Succeeded)
                {
                    if (model.Role != roles.First())
                    {
                        foreach (var role in roles)
                        {
                            await _userManager.RemoveFromRoleAsync(existUser, role);
                        }
                        await _userManager.AddToRoleAsync(existUser, model.Role);

                    }

                    return new Message<string>()
                    {
                        Status = "S",
                        Text = $"User {existUser.UserName} details updated successfully",
                        Result = existUser.Id.ToString()

                    };
                }
                return new Message<string>()
                { Text = $"Update error {result?.Errors?.FirstOrDefault()?.Description}" };
            }
            catch (Exception ex)
            {
                return new Message<string>()
                { Text = $"Update error {ex.Message}" };
            }
        }
        public async Task<Message<string>> RegisterAsync(InsertAccount model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                // LastUpdatedBy = User.Identities.First().Claims.Single(s => s.Type == "uid").Value,
                // LastUpdatedDateTime = DateTime.Now,
            };
            try
            {
                var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userWithSameEmail == null)
                {
                    user.UserName = await CheckADAsync(model.Email.ToLower().Trim());

                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                        return new Message<string>() { Status = "S", Text = $"User Registered with username {user.UserName}", Result = user.Id.ToString() };
                    }
                    else
                    {
                        return new Message<string>() { Text = result?.Errors?.First().Description };
                    }
                }
                return new Message<string>() { Text = $"Email {user.Email } is already registered." };
            }
            catch (Exception ex)
            {
                return new Message<string>() { Status = "E", Text = ex.Message };
            }
        }
        public async Task<bool> CheckEmailinADAsync(ApplicationUser user, string password)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, AD_Server_IP))
                {
                    var adUserName = user.Email.ToLower().Trim().Replace("@inseegroup.com", String.Empty);
                    var userPrincipal = UserPrincipal.FindByIdentity(pc, adUserName);
                    if (userPrincipal == null)
                    {
                        throw new Exception("User Unable to find in AD");
                    }

                    var username = userPrincipal.SamAccountName;
                    if (!string.IsNullOrEmpty(username))
                    {
                        return pc.ValidateCredentials(adUserName, password);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<bool> CheckUsernameinADAsync(string username, string password)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, AD_Server_IP))
                {
                    try
                    {
                        return pc.ValidateCredentials(username.ToLower().Trim(), password);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("User Unable to find in AD");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<string> CheckADAsync(string email)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, AD_Server_IP))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(pc, email.ToLower().Trim());
                if (userPrincipal == null)
                {
                    throw new Exception($"User {email} Unable to find in AD");
                }
                return userPrincipal.SamAccountName;
            }
            throw new Exception($"User {email} Unable to find in AD");
        }

        public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
        {
            var authenticationModel = new AuthenticationModel();
            var IsAuthorized = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(String.Format("{0}@siamcitycement.com", model.Email).ToLower());
                    if (user == null)
                    {

                        authenticationModel.IsAuthenticated = false;
                        authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
                        return authenticationModel;
                    }
                }


                //if (user.Email.ToLower().Trim().Equals("admin@overleap.com"))
                //{
                //    // overleadp
                //    if (await _userManager.CheckPasswordAsync(user, model.Password))
                //    {
                //        IsAuthorized = true;
                //    }
                //} 
                if(model.Email.Split('@').Count() == 1 && await CheckUsernameinADAsync(model.Email, model.Password))
                {
                    IsAuthorized = true;
                }
                else if (await CheckEmailinADAsync(user, model.Password))
                {
                    /// for AD
                    IsAuthorized = true;
                }

                if (IsAuthorized)
                {
                    if (user.LockoutEnabled && user.LockoutEnd > DateTime.Now)
                    {
                        authenticationModel.IsAuthenticated = false;
                        authenticationModel.Message = $"User account blocked, Please contact system admin";
                        return authenticationModel;
                    }

                    authenticationModel.IsAuthenticated = true;
                    JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                    authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    authenticationModel.Email = user.Email;
                    authenticationModel.UserName = user.UserName;
                    var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                    authenticationModel.Roles = rolesList.ToList();


                    if (user.RefreshTokens.Any(a => a.IsActive))
                    {
                        var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                        authenticationModel.RefreshToken = activeRefreshToken.Token;
                        authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
                    }
                    else
                    {
                        var refreshToken = CreateRefreshToken();
                        authenticationModel.RefreshToken = refreshToken.Token;
                        authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                        user.RefreshTokens.Add(refreshToken);
                        _context.Update(user);
                        _context.SaveChanges();
                    }

                    return authenticationModel;
                }
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
                return authenticationModel;
            }
            catch (Exception ex)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = ex.Message;
                return authenticationModel;
            }
        }

        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow
                };

            }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return $"No Accounts Registered with {model.Email}.";
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roleExists = Enum.GetNames(typeof(CommonResources.Roles)).Any(x => x.ToLower() == model.Role.ToLower());
                if (roleExists)
                {
                    var validRole = Enum.GetValues(typeof(CommonResources.Roles)).Cast<CommonResources.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                    await _userManager.AddToRoleAsync(user, validRole.ToString());
                    return $"Added {model.Role} to user {model.Email}.";
                }
                return $"Role {model.Role} not found.";
            }
            return $"Incorrect Credentials for user {user.Email}.";

        }

        public async Task<AuthenticationModel> RefreshTokenAsync(string token)
        {
            var authenticationModel = new AuthenticationModel();
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"Token did not match any users.";
                return authenticationModel;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"Token Not Active.";
                return authenticationModel;
            }

            //Revoke Current Refresh Token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            //Generates new jwt
            authenticationModel.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();
            authenticationModel.RefreshToken = newRefreshToken.Token;
            authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
            return authenticationModel;
        }
        public bool RevokeToken(string token)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            _context.Update(user);
            _context.SaveChanges();

            return true;
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public async Task<List<string>> GetRolesById(string id)
        {
            var result = await _userManager.GetRolesAsync(_context.Users.Find(id));
            return result.ToList<string>();
        }
        //TODO : Update User Details
        //TODO : Remove User from Role 
    }
}
