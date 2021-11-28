using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using sds.notificaciones.core.DTO;
using sds.notificaciones.core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace sds.notificaciones.infraestructure.Messaging
{
    public class KafkaConsumerHandler : BackgroundService
    {
        private readonly string topic = "notifications";
        public IServiceScopeFactory serviceScopeFactory;

        public KafkaConsumerHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            var conf = new ConsumerConfig
            {
                GroupId = "notifications_consumer_group",
                //BootstrapServers = "localhost:9092",
				BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_SERVER"),
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = Environment.GetEnvironmentVariable("KAFKA_USERNAME"),
                SaslPassword = Environment.GetEnvironmentVariable("KAFKA_PASSWORD"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore, 
                string>(conf).Build())
            {
                builder.Subscribe(topic);
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumer = builder.Consume(cancellationToken);
                        try 
                        {
                            var notificacion = JsonSerializer.Deserialize<Notificacion>(consumer.Message.Value);
                            using (var scope = serviceScopeFactory.CreateScope())
                            {
                                NotificacionService notificacionService = scope.ServiceProvider.GetRequiredService<NotificacionService>();
                                notificacionService.send(notificacion);
                            }
                        }
                        catch(JsonException ex) 
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}