using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.DTO;
using Antlr4.StringTemplate;
using sds.notificaciones.core.entities;
using System;
using Microsoft.Extensions.Logging;

namespace sds.notificaciones.core.services {
    public class NotificacionServiceImpl : NotificacionService
    {
        private readonly MailRepository mailRepository;
        private readonly MailClient mailClient;
        private readonly ILogger<NotificacionServiceImpl> logger;

        public NotificacionServiceImpl(ILogger<NotificacionServiceImpl> logger, MailRepository mailRepository, MailClient mailClient)
        {
            this.logger = logger;
            this.mailRepository = mailRepository;
            this.mailClient = mailClient;
        }
        public void send(Notificacion notificacion) 
        {
            var mail = GenerateMail(notificacion);
            mailClient.send(mail);
            mailRepository.Save(mail);
            logger.LogInformation("Notificacion enviada a " + notificacion.to);

        }

        private Mail GenerateMail(Notificacion notificacion) 
        {
            Template template = getTemplate(notificacion.template);
            if(notificacion.vars.ContainsKey("nombre")) {
                template.Add("nombre", notificacion.vars["nombre"]);
            }
            if(notificacion.vars.ContainsKey("tramiteId")) {
                template.Add("tramiteId", notificacion.vars["tramiteId"]);
            }
            string body = template.Render();
            var mail = new Mail {
                from = "castellanosmjuanc@javeriana.edu.co",
                to = notificacion.to,
                subject = "Ventanilla de trámites SDS - Estado de su trámite",
                body = body
            };
            return mail;
        }

        private Template getTemplate(string nombreTemplate) {
            Template template = null;
            if ("tramite-radicado".Equals(nombreTemplate, StringComparison.CurrentCultureIgnoreCase)) {
                template = new Template("Señor: <nombre>: Su tramite con código <tramiteId> fue radicado.");
            } else if ("tramite-aprobado-generado".Equals(nombreTemplate, StringComparison.CurrentCultureIgnoreCase)) {
                template = new Template("Señor: <nombre>: Su tramite con código <tramiteId> fue aprobado y ya se encuentra generada la resolución para su desgarga en la ventanilla de trámites.");
            } else if ("tramite-desaprobado-generado".Equals(nombreTemplate, StringComparison.CurrentCultureIgnoreCase)) {
                template = new Template("Señor: <nombre>: Su tramite con código <tramiteId> no fue aprobado y ya se encuentra generada la resolución para su desgarga en la ventanilla de trámites.");
            } else {
                template = new Template("Correo sin plantilla. Nombre template: " + nombreTemplate);
            }
            return template; 
        }
    }
}