using sds.notificaciones.core.entities;
using sds.notificaciones.core.DTO;

namespace sds.notificaciones.core.Interfaces {
    public interface TemplateService {
        Mail GenerateMail(Notificacion notificacion);
    }
}