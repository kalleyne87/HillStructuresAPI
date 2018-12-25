using System.Net.Mail;

namespace HillStructuresAPI.Services
{
    public class EmailSender
    {
        public void SendEmailAsync(string email, string Subject, string Message)
        {
            Execute(Subject, Message, email);
        }

        public void Execute(string Subject, string message, string email)
        {
            string fromEmail = "System@HillStructures.com";
            string toEmail = email;
            int smtpPort = 587;
            string smtpHost = "smtp.sendgrid.net";
            string smtpUser = "azure_d9bd2eeed776a8b66de6560f4ae4deb7@azure.com";
            string smtpPass = "67806Ker";
            string subject = Subject;
            string Message = message;

            MailMessage mail = new MailMessage(fromEmail, toEmail);
            SmtpClient client = new SmtpClient();
            client.Port = smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = smtpHost;
            client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}
