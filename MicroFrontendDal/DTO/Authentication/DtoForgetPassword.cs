/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace MicroFrontendDal.DTO.Authentication
{
    public class DtoForgetPassword
    {
        public string Email { get; set; }
    }
    public class DtoResetPassword
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
    }
    public class DtoResetPasswordAndVerifyEmail
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
    }
}
