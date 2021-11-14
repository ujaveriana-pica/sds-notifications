using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Antlr4.StringTemplate;
using SendGrid;
using SendGrid.Helpers.Mail;
using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.DTO;

namespace sds_notificaciones.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacionController : ControllerBase
    {
        private readonly NotificacionService notificacionService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<NotificacionController> _logger;

        public NotificacionController(ILogger<NotificacionController> logger, NotificacionService notificacionService )
        {
            _logger = logger;
            this.notificacionService = notificacionService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public Notificacion Send([FromBody] Notificacion notificacion) 
        {
            notificacionService.send(notificacion);
            return notificacion;
        }

        
    }
}
