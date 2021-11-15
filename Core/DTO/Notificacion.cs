using System.Collections.Generic;

namespace sds.notificaciones.core.DTO
{
    public class Notificacion 
    {
        public string template { get; set; } 
        public string to { get; set; }
        public Dictionary<string, string> vars  { get; set; }
    }
}