namespace sds.notificaciones.core.entities {
    public class Mail 
        {
            public long id { get; set; }
            public string from { get; set; } 
            public string to { get; set; }
            public string subject { get; set; } 
            public string body { get; set; } 
    }
}