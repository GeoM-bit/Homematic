using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

using System.Net.Mail;

namespace HomematicApp.Service.Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        public SenderOptions Options { get; }

        public EmailSender(IOptions<SenderOptions> senderOptions)
        {
            Options = senderOptions.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(Options.Email, Options.Password);
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Timeout = 2000;

                MailMessage message = new MailMessage(Options.Email, email);

                message.Body = htmlMessage;
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.None;

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
            }
        }
    }
}

