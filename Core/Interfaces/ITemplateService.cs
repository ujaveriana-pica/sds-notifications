using sds.notificaciones.core.entities;
using sds.notificaciones.core.DTO;

namespace sds.notificaciones.core.Interfaces {
    public interface ITemplateService {
        Mail GenerateMail(Notificacion notificacion);
    }
}