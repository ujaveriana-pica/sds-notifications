using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace sds.notificaciones.infraestructure.Clients
{
    public class MailClientSendGrid: MailClient
    {
        public void send(Mail mail) 
        {
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var apiKey = "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";           
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(mail.from);
            var toEmailAddress = new EmailAddress(mail.to);
            var plainTextContent = mail.body;
            var htmlContent = mail.body;
            var msg = MailHelper.CreateSingleEmail(from, toEmailAddress, mail.subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }
    }
}