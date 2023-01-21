using MicroFrontendDal.DTO.Authentication;

namespace MicroFrontendDal.BusinessRules.Authentication
{
    public interface IAuthentication
    {
        Task<DtoUserRegistrationResponse> RegisterUser(DtoRegisterUser dtouser);
        Task<DtoTokenResponse> LoginUser(DtoLoginUser dtouser);
        Task<string> ForgetPassword(DtoForgetPassword dtouser);
        Task<string> ResetPassword(DtoResetPassword dtouser);
        Task<DtoVerifyEmailResponse> VeriifyEmail(DtoVerifyEmail dtouser);
    }
}
