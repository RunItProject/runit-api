using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Runit.Backend.Services {
    public class EmailService {

        private SendGridClient client;
        public EmailService(IConfiguration configuration) {
            client = new SendGridClient(configuration["SendGridApiKey"]);
        }
        public async Task<bool> SendMail(string toEmail, string toName, string subject, string message) {
            var msg = new SendGridMessage() {
                From = new EmailAddress("noreply@runit.mortimer.nu", "RunIt"),
                TemplateId = "9d2739c8-23f7-48dd-9bce-7be2760034aa" 
            };
            
            msg.AddTo(new EmailAddress(toEmail, toName));
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, message);

            var response = await client.SendEmailAsync(msg);

            return response.StatusCode == HttpStatusCode.Accepted;
        }

        public async Task<bool> SendPasswordResetLink(string toEmail, string toName, string url) {
            var msg = new SendGridMessage() { 
                From = new EmailAddress("noreply@runit.mortimer.nu", "RunIt"),
                Subject = "Reset password",
                TemplateId = "3291d65a-ac51-442e-b359-74c30e3c9c04" 
            };
            
            msg.AddTo(new EmailAddress(toEmail, toName));
            msg.AddSubstitution("{{name}}", toName);
            msg.AddSubstitution("{{link}}", url);

            var response = await client.SendEmailAsync(msg);

            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}