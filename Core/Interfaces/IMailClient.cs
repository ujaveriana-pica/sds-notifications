using sds.notificaciones.core.entities;

namespace sds.notificaciones.core.Interfaces 
{
    public interface IMailClient
    {
        void Send(Mail mail);
    }
}