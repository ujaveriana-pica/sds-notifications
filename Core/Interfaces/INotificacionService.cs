using sds.notificaciones.core.DTO;

namespace sds.notificaciones.core.Interfaces
{
    public interface INotificacionService
    {
        public void send(Notificacion notificacion);
    }
}