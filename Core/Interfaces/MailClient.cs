using sds.notificaciones.core.entities;

namespace sds.notificaciones.core.Interfaces 
{
    public interface MailClient
    {
        void send(Mail mail);
    }
}