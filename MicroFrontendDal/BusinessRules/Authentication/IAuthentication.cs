using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Authentication;
using MicroFrontendDal.DTO.Common;

namespace MicroFrontendDal.BusinessRules.Authentication
{
    public interface IAuthentication
    {
        Task<DtoUserRegistrationResponse> RegisterUser(DtoRegisterUser dtouser);
        Task<DtoTokenResponse> LoginUser(DtoLoginUser dtouser);
        Task<string> ForgetPassword(DtoForgetPassword dtouser);
        Task<string> ResetPassword(DtoResetPassword dtouser);
        Task<DtoVerifyEmailResponse> VeriifyEmail(DtoVerifyEmail dtouser);
        List<MasterRole> GetAllActiveRoles();
        Task<DtoAdminRegisterNewUserResponse> AdminRegisterUser(DtoAdminRegisterNewUser dtouser);
        Task<DtoResponse> ResetPasswordAndConfirmEmail(DtoResetPasswordAndVerifyEmail dtouser);
        Task<DtoResponse> RemoveUser(int userId);
        Task<DtoUserRegistrationResponse> ResendActivationMail(string email);
    }
}
