using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using sds.notificaciones.core.DTO;
using sds.notificaciones.core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace sds.notificaciones.infraestructure.Messaging
{
    public class KafkaConsumerHandler : BackgroundService
    {
        private readonly string topic = "notifications";
        public IServiceScopeFactory serviceScopeFactory;
        private readonly ConsumerConfig consumerConfig;

        public KafkaConsumerHandler(IServiceScopeFactory serviceScopeFactory, ConsumerConfig consumerConfig)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.consumerConfig = consumerConfig;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            using (var builder = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
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
                                INotificacionService notificacionService = scope.ServiceProvider.GetRequiredService<INotificacionService>();
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