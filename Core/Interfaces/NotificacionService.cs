using sds.notificaciones.core.DTO;

namespace sds.notificaciones.core.Interfaces
{
    public interface NotificacionService
    {
        public void send(Notificacion notificacion);
    }
}