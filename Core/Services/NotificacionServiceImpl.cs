using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.DTO;
using Microsoft.Extensions.Logging;
using System;

namespace sds.notificaciones.core.services {
    public class NotificacionServiceImpl : INotificacionService
    {
        private readonly IMailRepository mailRepository;
        private readonly IMailClient mailClient;
        private readonly ITemplateService templateService;
        private readonly ILogger<NotificacionServiceImpl> logger;

        public NotificacionServiceImpl(ILogger<NotificacionServiceImpl> logger, IMailRepository mailRepository, 
            IMailClient mailClient, ITemplateService templateService)
        {
            this.logger = logger;
            this.mailRepository = mailRepository;
            this.mailClient = mailClient;
            this.templateService = templateService;
        }
        public void send(Notificacion notificacion) 
        {
            var mail = templateService.GenerateMail(notificacion);
            mailRepository.Save(mail);
            string sendMail = Environment.GetEnvironmentVariable("SEND_MAIL");
            if(sendMail != null && sendMail.Equals("true", StringComparison.CurrentCultureIgnoreCase)) 
            {
                mailClient.Send(mail);
                logger.LogInformation("Notificacion con plantilla " + notificacion.template + " enviada a " + notificacion.to);
            } 
            else {
                logger.LogInformation("Notificacion con plantilla " + notificacion.template + " enviada a " + notificacion.to + " - Envio de correo deshabilitado");
            }
        }

    }
}