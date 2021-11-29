using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.entities;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace sds.notificaciones.infraestructure.Clients
{
    public class MailClientSendGrid: MailClient
    {
        public void Send(Mail mail) 
        {
            var apiKey = Environment.GetEnvironmentVariable("SENGRID_KEY");
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