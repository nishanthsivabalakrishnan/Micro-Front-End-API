using MicroFrontendDal.BusinessRules.Logger;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.BusinessConstants;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendDal.BusinessRules.Email
{
    public class Email
    {
        #region Email
        private readonly Log log = new();
        public bool SendEmail(string toUser, string subject, string body)
        {
            try
            {
                MicroFrontEndDbContext dbContext = new();
                var userEmail = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.CommunicationEmail);
                var userPassword = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.CommunicationEmailPassword);
                var SmtpHost = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.SMTPHost);
                var SmtpPort = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.SMTPPort);

                if (userEmail != null && userPassword != null && SmtpPort != null && SmtpHost != null)
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(userEmail.Value));
                    email.To.Add(MailboxAddress.Parse(toUser));
                    email.Subject = subject;
                    email.Body = new TextPart(TextFormat.Plain) { Text = body };
                    var smtp = new SmtpClient();
                    smtp.Connect(SmtpHost.Value, int.Parse(SmtpPort.Value), SecureSocketOptions.StartTls);
                    smtp.Authenticate(userEmail.Value, userPassword.Value);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.ErrorLog("Email", "SendEmail", ex);
                return false;
            }
        }
        #endregion

        #region Welcome Email
        public bool WelcomeEmail(string toUser, string subject, string resettoken)
        {
            try
            {
                MicroFrontEndDbContext dbContext = new();
                var userEmail = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.CommunicationEmail);
                var userPassword = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.CommunicationEmailPassword);
                var SmtpHost = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.SMTPHost);
                var SmtpPort = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.SMTPPort);
                var senderDisplayName = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.DisplayName);
                var welcomeTemplate = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.WelcomeTemplate);
                var url = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.FrontEndUrl);
                if ((userEmail == null || userPassword == null || SmtpPort == null) || welcomeTemplate == null)
                {
                    return false;
                }
                var BuildUrl = url.Value + "welcome?email=" + toUser + "&id=" + resettoken;
                string body = welcomeTemplate.Value.Replace(Common.ConfirmEmailReplaceText, BuildUrl);

                if (SmtpHost != null && senderDisplayName != null && body != string.Empty)
                {
                    var email = new MimeMessage();
                    String from = "\"temp\" <" + userEmail.Value + ">";
                    from = from.Replace("temp", senderDisplayName.Value);
                    email.From.Add(MailboxAddress.Parse(from));
                    email.To.Add(MailboxAddress.Parse(toUser));
                    email.Subject = subject;
                    email.Body = new TextPart(TextFormat.Html) { Text = body };
                    var smtp = new SmtpClient();
                    smtp.Connect(SmtpHost.Value, int.Parse(SmtpPort.Value), SecureSocketOptions.StartTls);
                    smtp.Authenticate(userEmail.Value, userPassword.Value);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.ErrorLog("Email", "WelcomeEmail", ex);
                return false;
            }
        }
        #endregion

        #region Admin Add User Welcome Mail
        public bool AdminAddUserWelcomeEmail(string toUser, string subject, string resettoken)
        {
            try
            {
                MicroFrontEndDbContext dbContext = new();
                var userEmail = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.CommunicationEmail);
                var userPassword = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.CommunicationEmailPassword);
                var SmtpHost = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.SMTPHost);
                var SmtpPort = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.SMTPPort);
                var senderDisplayName = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.DisplayName);
                var welcomeTemplate = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.WelcomeTemplate);
                var url = dbContext.MasterInformations.FirstOrDefault(x => x.MasterId == BusinessConstant.MasterInformation.FrontEndResetPasswordUrl);
                if ((userEmail == null || userPassword == null || SmtpPort == null) || welcomeTemplate == null)
                {
                    return false;
                }
                var BuildUrl = url.Value + "welcome?email=" + toUser + "&rt=" + resettoken;
                string body = welcomeTemplate.Value.Replace(Common.ConfirmEmailReplaceText, BuildUrl);

                if (SmtpHost != null && senderDisplayName != null && body != string.Empty)
                {
                    var email = new MimeMessage();
                    String from = "\"temp\" <" + userEmail.Value + ">";
                    from = from.Replace("temp", senderDisplayName.Value);
                    email.From.Add(MailboxAddress.Parse(from));
                    email.To.Add(MailboxAddress.Parse(toUser));
                    email.Subject = subject;
                    email.Body = new TextPart(TextFormat.Html) { Text = body };
                    var smtp = new SmtpClient();
                    smtp.Connect(SmtpHost.Value, int.Parse(SmtpPort.Value), SecureSocketOptions.StartTls);
                    smtp.Authenticate(userEmail.Value, userPassword.Value);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.ErrorLog("Email", "AdminAddUserWelcomeEmail", ex);
                return false;
            }
        }

        #endregion
    }
}
