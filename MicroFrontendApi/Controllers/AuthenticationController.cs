using MicroFrontendDal.BusinessRules.Authentication;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DTO.Authentication;
using MicroFrontendDal.DTO.Common;
using MicroFrontendDal.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendApi.Controllers
{
    [Route("Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        protected IAuthentication UserRepository;
        private readonly Log Logger;
        public AuthenticationController(IAuthentication userRepository, Log logger)
        {
            UserRepository = userRepository;
            Logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                if (decryptedData != null)
                {
                    DtoRegisterUser data = JsonConvert.DeserializeObject<DtoRegisterUser>(decryptedData);
                    var Response = await UserRepository.RegisterUser(data);
                    var jsonResponse = JsonConvert.SerializeObject(Response);
                    if (Response.Status == Status.UserCreatedStatus)
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status501NotImplemented, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "Register", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost]
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                if (decryptedData != null)
                {
                    DtoLoginUser data = JsonConvert.DeserializeObject<DtoLoginUser>(decryptedData);
                    var Response = await UserRepository.LoginUser(data);
                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    if (Response.Token != string.Empty)
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "LoginUser", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(DtoVerifyEmail objInputData)
        {
            try
            {
                var Response = await UserRepository.VeriifyEmail(objInputData);
                string jsonResponse = JsonConvert.SerializeObject(Response);
                if (Response.Status == ResponseStatus.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "LoginUser", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(DtoForgetPassword objInputData)
        {
            try
            {
                var Response = await UserRepository.ForgetPassword(objInputData);
                if (Response == CustomMessages.CM009)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, Response);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "ResetPassword", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(DtoResetPassword objInputData)
        {
            try
            {
                var Response = await UserRepository.ResetPassword(objInputData);
                if (Response == CustomMessages.CM006)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, Response);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "ResetPassword", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var Response = UserRepository.GetAllActiveRoles();
                var jsonResponse = JsonConvert.SerializeObject(Response);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetAllRoles", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost]
        [Route("AdminRegisterNewUser")]
        public async Task<IActionResult> AdminRegisterNewUser(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                if (decryptedData != null)
                {
                    DtoAdminRegisterNewUser data = JsonConvert.DeserializeObject<DtoAdminRegisterNewUser>(decryptedData);
                    var Response = await UserRepository.AdminRegisterUser(data);
                    var jsonResponse = JsonConvert.SerializeObject(Response);
                    if (Response.Status == ResponseStatus.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "AdminRegisterNewUser", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost]
        [Route("ResetPasswordAndConfirmEmail")]
        public async Task<IActionResult> ResetPasswordAndConfirmEmail(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                if (decryptedData != null)
                {
                    DtoResetPasswordAndVerifyEmail data = JsonConvert.DeserializeObject<DtoResetPasswordAndVerifyEmail>(decryptedData);
                    var Response = await UserRepository.ResetPasswordAndConfirmEmail(data);
                    var jsonResponse = JsonConvert.SerializeObject(Response);
                    if (Response.Status == ResponseStatus.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "ResetPasswordAndConfirmEmail", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost]
        [Route("RemoveUser")]
        public async Task<IActionResult> RemoveUser(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                if (decryptedData != null)
                {
                    int userId = JsonConvert.DeserializeObject<int>(decryptedData);
                    var Response = await UserRepository.RemoveUser(userId);
                    var jsonResponse = JsonConvert.SerializeObject(Response);
                    if (Response.Status == ResponseStatus.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "ResetPasswordAndConfirmEmail", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost]
        [Route("ResendActivationMail")]
        public async Task<IActionResult> ResendActivationMail(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                DtoRegisterUser data = JsonConvert.DeserializeObject<DtoRegisterUser>(decryptedData);
                if (objInputData != null)
                {
                    var Response = await UserRepository.ResendActivationMail(data.Email);
                    var jsonResponse = JsonConvert.SerializeObject(Response);
                    if (Response.Status == Status.UserResendActivationMailSent)
                    {
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status501NotImplemented, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "Register", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }
    }
}
