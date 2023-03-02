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
            public static readonly string CM015 = "Password set and Email verified Successfully!";
            public static readonly string CM016 = "Password set and Email verified failed!";
            public static readonly string CM017 = "User Details Updated Successfully!";
            public static readonly string CM018 = "User Deleted Successfully!";
            public static readonly string CM019 = "Failed to Delete user!";
            public static readonly string CM020 = "Task Created Successfully!";
            public static readonly string CM021 = "Task Updated Successfully!";
            public static readonly string CM022 = "Failed to Create task!";
            public static readonly string CM023 = "Task deleted Successfully!";
            public static readonly string CM024 = "Failed to delete task!";
            public static readonly string CM025 = "Task status updated successfully!";
            public static readonly string CM026 = "Failed to update Task status!";
            public static readonly string CM027 = "Resend Activation mail sent!";
            public static readonly string CM028 = "Failed to sent resend activation mail!";
            public static readonly string CM029 = "Profile Information Updated Successfully!";
            public static readonly string CM030 = "Failed to update Profile Information!";
        }
        public static class Status
        {
            public static readonly int NewUser = 1;
            public static readonly int UserCreatedStatus = 2;
            public static readonly int UserNotCreatedStatus = 3;
            public static readonly int UserResendActivationMailSent = 4;
        }
        public static class MasterUserStatus
        {
            public static readonly int NewUser = 1;
            public static readonly int VerifiedAndPasswordChanged = 2;
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
            public static readonly int FrontEndResetPasswordUrl = 8;
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
