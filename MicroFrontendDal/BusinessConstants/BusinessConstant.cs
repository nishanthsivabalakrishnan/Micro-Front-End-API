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
            public static readonly string CM010 = "Application is restarting!";
            public static readonly string CM011 = "Application is already running!";
            public static readonly string CM012 = "Data Sent to Epos Buddy Product Api successfully!";
            public static readonly string CM013 = "Sending data to Epos Buddy Product Api failed!";
        }
        public static class Status
        {
            public static readonly int NewUser = 1;
        }
        public static class MasterInformation
        {
            public static readonly int CommunicationEmail = 1;
            public static readonly int CommunicationEmailPassword = 2;
            public static readonly int SMTPHost = 3;
            public static readonly int SMTPPort = 4;
            public static readonly int WelcomeTemplate = 5;
            public static readonly int DisplayName = 6;
        }
        public static class Common
        {
            public static readonly string ConfirmEmailReplaceText = "{targetUrl}";
        }
    }
    #endregion

}
