using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.DTO;
using Antlr4.StringTemplate;
using sds.notificaciones.core.entities;

namespace sds.notificaciones.core.services {
    public class NotificacionServiceImpl : NotificacionService
    {
        private readonly MailRepository mailRepository;
        private readonly MailClient mailClient;

        public NotificacionServiceImpl(MailRepository mailRepository, MailClient mailClient)
        {
            this.mailRepository = mailRepository;
            this.mailClient = mailClient;
        }
        public void send(Notificacion notificacion) 
        {
            var mail = GenerateMail(notificacion);
            mailClient.send(mail);
            mailRepository.Save(mail);
        }

        private Mail GenerateMail(Notificacion notificacion) 
        {
            Template template = new Template("Señor: <nombre>: Su tramite con código <tramiteId> fue radicado.");
            template.Add("nombre", "Juan Carlos");
            template.Add("tramiteId", "123456");
            string body = template.Render();
            var mail = new Mail {
                from = "castellanosmjuanc@javeriana.edu.co",
                to = notificacion.to,
                subject = "El subject del correo",
                body = body
            };
            return mail;
        }
    }
}