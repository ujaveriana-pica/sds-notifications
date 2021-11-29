using sds.notificaciones.core.entities;

namespace sds.notificaciones.core.Interfaces 
{
    public interface IMailRepository
    {
        void Save(Mail mail);
    }
}