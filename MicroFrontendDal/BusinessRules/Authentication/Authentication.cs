using MicroFrontendDal.BusinessRules.AppDbContext;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
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
        private readonly Utilities.Utilities Utilities;
        #endregion

        #region Constructor
        public Authentication(UserManager<IdentityUser> userManager)
        {
            DbContext = new MicroFrontEndDbContext();
            UserManager = userManager;
            Logger = new Log();
            Utilities = new Utilities.Utilities();
        }
        #endregion

        #region Register New User
        public async Task<DtoUserRegistrationResponse> RegisterUser(DtoRegisterUser dtouser)
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
                        User objUser = new();
                        var netusers = await UserManager.FindByEmailAsync(dtouser.Email);
                        var resetToken = await UserManager.GenerateEmailConfirmationTokenAsync(netusers);
                        objUser.AuthId = netusers.Id;
                        objUser.Role = MasterRoleId.User;
                        objUser.Email = dtouser.Email;
                        objUser.Status = Status.NewUser;
                        objUser.CreatedOn = DateTime.UtcNow;
                        objUser.IsDelete = false;
                        DbContext.Users.Add(objUser);
                        DbContext.SaveChanges();
                        Email.Email mail = new();
                        var sendMailThread = new Thread(() => mail.WelcomeEmail(objUser.Email, "Welcome to PoC", HttpUtility.UrlEncode(resetToken)));
                        sendMailThread.Start();
                        DtoUserRegistrationResponse response = new()
                        {
                            Message = CustomMessages.CM001,
                            Status = Status.UserCreatedStatus
                        };
                        return response;
                    }
                }
                DtoUserRegistrationResponse ErrorResponse = new()
                {
                    Message = CustomMessages.CM002,
                    Status = Status.UserNotCreatedStatus
                };
                return ErrorResponse;
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
                    var UserDetail = DbContext.Users.FirstOrDefault(x => x.AuthId == User.Id);
                    if (!await UserManager.IsEmailConfirmedAsync(User))
                    {
                        Response = new DtoTokenResponse()
                        {
                            Token = string.Empty,
                            Message = CustomMessages.CM010
                        };
                        return Response;
                    }
                    if (await UserManager.CheckPasswordAsync(User, dtouser.Password) && UserDetail != null)
                    {
                        var claims = new[] {
                        new Claim("UserId", UserDetail.UserId.ToString()),
                        new Claim("Email", UserDetail.Email),
                        new Claim("Role", UserDetail.Role.ToString()),
                        };

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var Token = new JwtSecurityToken(issuer: configuration["JWT:ValidIssuer"], audience: configuration["JWT:ValidAudience"],
                            claims, expires: DateTime.Now.AddMinutes(180), signingCredentials: credentials);
                        var TokenSting = new JwtSecurityTokenHandler().WriteToken(Token);
                        Response = new DtoTokenResponse()
                        {
                            Token = TokenSting,
                            Message = CustomMessages.CM004,
                            ValidTill = Token.ValidTo
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

        #region Forget Password
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

        #region Verify Email
        public async Task<DtoVerifyEmailResponse> VeriifyEmail(DtoVerifyEmail dtouser)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(dtouser.Email);
                if (user != null)
                {
                    var verification = await UserManager.ConfirmEmailAsync(user, dtouser.Token);
                    if(verification.Succeeded)
                    {
                        DtoVerifyEmailResponse response = new DtoVerifyEmailResponse()
                        {
                            Status = ResponseStatus.Success,
                            Message = CustomMessages.CM011
                        };
                        return response;
                    }
                    else
                    {
                        DtoVerifyEmailResponse response = new DtoVerifyEmailResponse()
                        {
                            Status = ResponseStatus.Failed,
                            Message = CustomMessages.CM012
                        };
                        return response;
                    }
                }
                else
                {
                    DtoVerifyEmailResponse response = new DtoVerifyEmailResponse()
                    {
                        Status = ResponseStatus.Failed,
                        Message = CustomMessages.CM005
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "VeriifyEmail", ex);
                throw;
            }
        }

        #endregion
    }
}
