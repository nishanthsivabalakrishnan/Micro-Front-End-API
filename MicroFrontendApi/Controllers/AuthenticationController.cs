using MicroFrontendDal.BusinessRules.Authentication;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendApi.Controllers
{
    [Route("Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        protected IAuthentication UserRepository;
        private readonly Log Logger;
        public AuthenticationController(IAuthentication userRepository)
        {
            UserRepository = userRepository;
            Logger = new Log();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(DtoRegisterUser objInputData)
        {
            try
            {
                var Response = await UserRepository.RegisterUser(objInputData);
                if (Response == CustomMessages.CM001)
                {
                    return StatusCode(StatusCodes.Status200OK, new { Message = CustomMessages.CM001 });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { Message = CustomMessages.CM002 });
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
        public async Task<IActionResult> LoginUser(DtoLoginUser objInputData)
        {
            try
            {
                var Response = await UserRepository.LoginUser(objInputData);
                if (Response.Token != string.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, Response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Response);
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
    }
}
