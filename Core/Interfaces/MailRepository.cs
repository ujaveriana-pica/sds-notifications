using sds.notificaciones.core.entities;

namespace sds.notificaciones.core.Interfaces 
{
    public interface MailRepository
    {
        void Save(Mail mail);
    }
}