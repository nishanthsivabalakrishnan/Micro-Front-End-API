using MicroFrontendDal.BusinessRules.AppDbContext;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Authentication;
using MicroFrontendDal.DTO.Common;
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
        #endregion

        #region Constructor
        public Authentication(UserManager<IdentityUser> userManager, Log log)
        {
            DbContext = new MicroFrontEndDbContext();
            UserManager = userManager;
            Logger = log;
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
                        new Claim("Role",UserDetail.Role.ToString()),
                        new Claim(ClaimTypes.Role, UserDetail.Role.ToString()),
                        new Claim("Name",UserDetail.FirstName+" "+UserDetail.LastName)
                        };

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var Token = new JwtSecurityToken(issuer: configuration["JWT:ValidIssuer"], audience: configuration["JWT:ValidAudience"],
                            claims, expires: DateTime.Now.AddMinutes(180), signingCredentials: credentials);
                        var TokenSting = new JwtSecurityTokenHandler().WriteToken(Token);
                        var route = DbContext.MasterPageRoutings.FirstOrDefault(x => x.RoleId == UserDetail.Role && x.IsDelete == false);
                        Response = new DtoTokenResponse()
                        {
                            Status = ResponseStatus.Success,
                            Token = TokenSting,
                            Message = CustomMessages.CM004,
                            ValidTill = Token.ValidTo,
                            Route = route.MasterPageRoute
                        };
                    }
                    else
                    {
                        Response = new DtoTokenResponse()
                        {
                            Status = ResponseStatus.Failed,
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
                        Status = ResponseStatus.Failed,
                        Token = string.Empty,
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
                    if (verification.Succeeded)
                    {
                        DtoVerifyEmailResponse successResponse = new DtoVerifyEmailResponse()
                        {
                            Status = ResponseStatus.Success,
                            Message = CustomMessages.CM011
                        };
                        return successResponse;
                    }
                }
                DtoVerifyEmailResponse response = new DtoVerifyEmailResponse()
                {
                    Status = ResponseStatus.Failed,
                    Message = CustomMessages.CM005
                };
                return response;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "VeriifyEmail", ex);
                throw;
            }
        }

        #endregion

        #region Admin Add New User
        public async Task<DtoAdminRegisterNewUserResponse> AdminRegisterUser(DtoAdminRegisterNewUser dtouser)
        {
            try
            {
                var IdentityUser = await UserManager.FindByEmailAsync(dtouser.Email);

                if (!string.IsNullOrWhiteSpace(dtouser.Email) && IdentityUser == null && dtouser.UserId == 0)
                {
                    var user = new ApplicationUser { UserName = dtouser.Email, Email = dtouser.Email };
                    string randomPassword = Utilities.Utilities.CreateRandomPassword();
                    var UserCreated = await UserManager.CreateAsync(user, randomPassword);
                    if (UserCreated.Succeeded)
                    {
                        User objUser = new();
                        var netusers = await UserManager.FindByEmailAsync(dtouser.Email);
                        var resetToken = await UserManager.GeneratePasswordResetTokenAsync(netusers);
                        objUser.AuthId = netusers.Id;
                        objUser.Role = dtouser.Role;
                        objUser.Email = dtouser.Email;
                        objUser.ReportsTo = dtouser.ReportsTo;
                        objUser.FirstName = dtouser.FirstName;
                        objUser.LastName = dtouser.LastName;
                        objUser.Status = Status.NewUser;
                        objUser.CreatedOn = DateTime.UtcNow;
                        objUser.IsDelete = false;
                        DbContext.Users.Add(objUser);
                        DbContext.SaveChanges();
                        Email.Email mail = new();
                        var sendMailThread = new Thread(() =>
                        mail.AdminAddUserWelcomeEmail(objUser.Email, "Welcome to PoC - Activate Email", HttpUtility.UrlEncode(resetToken)));
                        sendMailThread.Start();
                        DtoAdminRegisterNewUserResponse successResponse = new()
                        {
                            Message = CustomMessages.CM001,
                            Status = ResponseStatus.Success
                        };
                        return successResponse;
                    }
                }
                else
                {
                    User objUser = DbContext.Users.FirstOrDefault(x => x.UserId == dtouser.UserId);
                    objUser.Role = dtouser.Role;
                    objUser.Email = dtouser.Email;
                    objUser.FirstName = dtouser.FirstName;
                    objUser.LastName = dtouser.LastName;
                    objUser.ReportsTo = dtouser.ReportsTo;
                    objUser.IsDelete = false;
                    DbContext.SaveChanges();
                    DtoAdminRegisterNewUserResponse successResponse = new()
                    {
                        Message = CustomMessages.CM017,
                        Status = ResponseStatus.Success
                    };
                    return successResponse;
                }
                DtoAdminRegisterNewUserResponse response = new()
                {
                    Message = CustomMessages.CM002,
                    Status = ResponseStatus.Failed
                };
                return response;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "AdminRegisterUser", ex);
                throw;
            }
        }
        #endregion

        #region Retrive All the Roles

        public List<MasterRole> GetAllActiveRoles()
        {
            try
            {
                var allRoles = DbContext.MasterRoles.Where(x => x.IsDelete == false).ToList();
                return allRoles;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetAllActiveRoles", ex);
                throw;
            }
        }

        #endregion

        #region Reset and Confirm Email
        public async Task<DtoResponse> ResetPasswordAndConfirmEmail(DtoResetPasswordAndVerifyEmail dtouser)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(dtouser.Email);
                var resetPassResult = await UserManager.ResetPasswordAsync(user, dtouser.ResetToken, dtouser.Password);
                user = await UserManager.FindByEmailAsync(user.Email);
                var resetToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                if (resetPassResult.Succeeded)
                {
                    var verification = await UserManager.ConfirmEmailAsync(user, resetToken);
                    if (verification.Succeeded)
                    {
                        var userDetails = DbContext.Users.FirstOrDefault(x => x.AuthId == user.Id);
                        userDetails.Status = MasterUserStatus.VerifiedAndPasswordChanged;
                        userDetails.UpdatedOn= DateTime.Now;
                        DbContext.SaveChanges();
                        DtoResponse successResponse = new()
                        {
                            Status = ResponseStatus.Success,
                            Message = CustomMessages.CM015,
                        };
                        return successResponse;
                    }
                }
                DtoResponse response = new()
                {
                    Status = ResponseStatus.Failed,
                    Message = CustomMessages.CM016,
                };
                return response;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "ResetPasswordAndConfirmEmail", ex);
                throw;
            }
        }
        #endregion

        #region Delete User
        public async Task<DtoResponse> RemoveUser(int userId)
        {
            try
            {
                var userDetails = DbContext.Users.FirstOrDefault(x => x.UserId == userId);
                if (userDetails != null)
                {
                    var netUser = await UserManager.FindByEmailAsync(userDetails.Email);
                    await UserManager.DeleteAsync(netUser);
                    userDetails.IsDelete = true;
                    DbContext.SaveChanges();
                    DtoResponse successResponse = new()
                    {
                        Status = ResponseStatus.Success,
                        Message = CustomMessages.CM018,
                    };
                    return successResponse;
                }
                DtoResponse failureResponse = new()
                {
                    Status = ResponseStatus.Failed,
                    Message = CustomMessages.CM019,
                };
                return failureResponse;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "RemoveUser", ex);
                throw;
            }
        }
        #endregion

        #region ResendActivationMail
        public async Task<DtoUserRegistrationResponse> ResendActivationMail(string email)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var IdentityUser = await UserManager.FindByEmailAsync(email);
                    var netusers = await UserManager.FindByEmailAsync(email);
                    var resetToken = await UserManager.GeneratePasswordResetTokenAsync(netusers);
                    Email.Email mail = new();
                    var sendMailThread = new Thread(() => mail.AdminAddUserWelcomeEmail(IdentityUser.Email, "Welcome to PoC", HttpUtility.UrlEncode(resetToken)));
                    sendMailThread.Start();
                    DtoUserRegistrationResponse response = new()
                    {
                        Message = CustomMessages.CM027,
                        Status = Status.UserResendActivationMailSent
                    };
                    return response;
                }
                DtoUserRegistrationResponse ErrorResponse = new()
                {
                    Message = CustomMessages.CM028,
                    Status = Status.UserNotCreatedStatus
                };
                return ErrorResponse;
            }
            catch(Exception ex)
            {
                Logger.ErrorLog(FileName, "ResendActivationMail", ex);
                throw;
            }
        }
        #endregion
    }
}
