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
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly string topic = "notificaciones";
        public IServiceScopeFactory serviceScopeFactory;

        public KafkaConsumerHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "notificaciones_consumer_group",
                BootstrapServers = "localhost:9092",
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
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}