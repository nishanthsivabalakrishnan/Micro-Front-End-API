namespace MicroFrontendDal.BusinessConstants
{
    #region Constant Region

    public static class BusinessConstant
    {
        public static class ErrorResponse
        {
            public static readonly string ErrorResponseMessage = "Aww!,An internal error occured.";
        }
        public static class CustomMessages
        {
            public static readonly string CM001 = "User Created Successfully!";
            public static readonly string CM002 = "User Already Exists!";
            public static readonly string CM003 = "Aww!,An internal error occured.";
            public static readonly string CM004 = "Login Successful!";
            public static readonly string CM005 = "User not found!";
            public static readonly string CM006 = "Failed to reset Password!";
            public static readonly string CM007 = "Password reset Successful!";
            public static readonly string CM008 = "Login failed.Please Check Email and Password!";
            public static readonly string CM009 = "User not found!";
            public static readonly string CM010 = "Please check your email and confirm your email!";
            public static readonly string CM011 = "Email verification completed!";
            public static readonly string CM012 = "Couldn't verify your Email!";
            public static readonly string CM013 = "Category Added Successfully!";
            public static readonly string CM014 = "Adding Category failed!";
        }
        public static class Status
        {
            public static readonly int NewUser = 1;
            public static readonly int UserCreatedStatus = 2;
            public static readonly int UserNotCreatedStatus = 3;
        }
        public static class MasterInformation
        {
            public static readonly int CommunicationEmail = 1;
            public static readonly int CommunicationEmailPassword = 2;
            public static readonly int SMTPHost = 3;
            public static readonly int SMTPPort = 4;
            public static readonly int WelcomeTemplate = 5;
            public static readonly int DisplayName = 6;
            public static readonly int FrontEndUrl = 7;
        }
        public static class Common
        {
            public static readonly string ConfirmEmailReplaceText = "{targetUrl}";
        }
        public static class ResponseStatus
        {
            public static readonly int Success = 1;
            public static readonly int Failed = 2;
        }
        public static class MasterRoleId
        {
            public static readonly int Admin = 1;
            public static readonly int User = 2;

        }
    }
    #endregion

}
