using MicroFrontendDal.BusinessRules.AppDbContext;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendDal.BusinessRules.Authentication
{
    public class Authentication : IAuthentication
    {
        #region Variable Region
        public MicroFrontEndDbContext DbContext { get; set; }

        private const string FileName = "Authentication";
        private readonly UserManager<IdentityUser> UserManager;
        private readonly Log Logger;
        #endregion

        #region Constructor
        public Authentication(UserManager<IdentityUser> userManager)
        {
            DbContext = new MicroFrontEndDbContext();
            UserManager = userManager;
            Logger = new Log();
        }
        #endregion

        #region Register New User
        public async Task<string> RegisterUser(DtoRegisterUser dtouser)
        {
            try
            {
                var IdentityUser = await UserManager.FindByEmailAsync(dtouser.Email);

                if (!string.IsNullOrWhiteSpace(dtouser.Email) && IdentityUser == null)
                {
                    var user = new ApplicationUser { UserName = dtouser.Email, Email = dtouser.Email };
                    var UserCreated = await UserManager.CreateAsync(user, dtouser.Password);
                    if (UserCreated.Succeeded)
                    {
                        User objUser = new User();
                        var netusers = await UserManager.FindByEmailAsync(dtouser.Email);
                        objUser.AuthId = netusers.Id;
                        objUser.FirstName = dtouser.FirstName;
                        objUser.LastName = dtouser.LastName;
                        objUser.Role = dtouser.Role;
                        objUser.UserName = dtouser.FirstName + dtouser.LastName;
                        objUser.Email = dtouser.Email;
                        objUser.Status = Status.NewUser;
                        objUser.CreatedOn = DateTime.UtcNow;
                        objUser.IsDelete = false;
                        DbContext.Users.Add(objUser);
                        DbContext.SaveChanges();
                        BusinessRules.Email.Email mail = new BusinessRules.Email.Email();
                        var sendMailThread = new Thread(() => mail.WelcomeEmail(objUser.Email,"Welcome to PoC"));
                        sendMailThread.Start();
                        return CustomMessages.CM001;
                    }
                }
                return CustomMessages.CM002;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "RegisterUser", ex);
                throw;
            }
        }
        #endregion

        #region Login User
        public async Task<DtoTokenResponse> LoginUser(DtoLoginUser dtouser)
        {
            try
            {
                DtoTokenResponse Response = new DtoTokenResponse();
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string jwtSecret = configuration["JWT:IssuerSignInKey"];

                var User = await UserManager.FindByEmailAsync(dtouser.Email);
                if (User != null)
                {
                    var UserDetail = DbContext.Users.FirstOrDefault(x => x.Email == dtouser.Email);

                    if (await UserManager.CheckPasswordAsync(User, dtouser.Password) && UserDetail != null)
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var Token = new JwtSecurityToken(
                            issuer: configuration["JWT:ValidIssuer"],
                            audience: configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddMinutes(120),
                            signingCredentials: credentials
                            );
                        var TokenSting = new JwtSecurityTokenHandler().WriteToken(Token);
                        Response = new DtoTokenResponse()
                        {
                            Token = TokenSting,
                            Message = CustomMessages.CM004
                        };
                    }
                    else
                    {
                        Response = new DtoTokenResponse()
                        {
                            Token = string.Empty,
                            Message = CustomMessages.CM008
                        };
                    }
                    return Response;
                }
                else
                {
                    Response = new DtoTokenResponse()
                    {
                        Token = "",
                        Message = CustomMessages.CM005
                    };
                    return Response;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "RegisterUser", ex);
                throw;
            }
        }

        #endregion

        #region ForgetPassword Password
        public async Task<string> ForgetPassword(DtoForgetPassword dtouser)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(dtouser.Email);
                if (user == null)
                {
                    return CustomMessages.CM009;
                }
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                return token;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "ForgetPassword", ex);
                throw;
            }
        }
        #endregion

        #region Reset Password
        public async Task<string> ResetPassword(DtoResetPassword dtouser)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(dtouser.Email);
                var resetPassResult = await UserManager.ResetPasswordAsync(user, dtouser.ResetToken, dtouser.Password);
                if (resetPassResult.Succeeded)
                {
                    return CustomMessages.CM007;
                }
                else
                {
                    return CustomMessages.CM007;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "ResetPassword", ex);
                throw;
            }
        }
        #endregion
    }
}
