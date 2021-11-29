using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.DTO;
using Microsoft.Extensions.Logging;

namespace sds.notificaciones.core.services {
    public class NotificacionServiceImpl : NotificacionService
    {
        private readonly MailRepository mailRepository;
        private readonly MailClient mailClient;
        private readonly TemplateService templateService;
        private readonly ILogger<NotificacionServiceImpl> logger;

        public NotificacionServiceImpl(ILogger<NotificacionServiceImpl> logger, MailRepository mailRepository, 
            MailClient mailClient, TemplateService templateService)
        {
            this.logger = logger;
            this.mailRepository = mailRepository;
            this.mailClient = mailClient;
            this.templateService = templateService;
        }
        public void send(Notificacion notificacion) 
        {
            var mail = templateService.GenerateMail(notificacion);
            mailClient.Send(mail);
            mailRepository.Save(mail);
            logger.LogInformation("Notificacion enviada a " + notificacion.to);

        }

    }
}