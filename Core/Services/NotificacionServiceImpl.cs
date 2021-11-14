using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.DTO;
using Antlr4.StringTemplate;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using SendGrid;
using sds.notificaciones.core.entities;

namespace sds.notificaciones.core.services {
    public class NotificacionServiceImpl : NotificacionService
    {
        public void send(Notificacion notificacion) 
        {
            var mail = GenerateMail(notificacion);
            Execute(mail).Wait();
        }

        private Mail GenerateMail(Notificacion notificacion) 
        {
            Template template = new Template("Señor: <nombre>: Su tramite con código <tramiteId> fue radicado.");
            template.Add("nombre", "Juan Carlos");
            template.Add("tramiteId", "123456");
            System.Console.Write("-------------- GenerateBody");
            string body = template.Render();
            var mail = new Mail {
                from = "castellanosmjuanc@javeriana.edu.co",
                to = notificacion.to,
                subject = "El subject del correo",
                body = body
            };
            return mail;
        }

        private async Task Execute(Mail mail)
        {
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var apiKey = "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";           
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(mail.from);
            var toEmailAddress = new EmailAddress(mail.to);
            var plainTextContent = mail.body;
            var htmlContent = mail.body;
            var msg = MailHelper.CreateSingleEmail(from, toEmailAddress, mail.subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}