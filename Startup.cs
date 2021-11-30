using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.services;
using sds.notificaciones.infraestructure.repositories;
using sds.notificaciones.infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using sds.notificaciones.infraestructure.Clients;
using sds.notificaciones.infraestructure.Messaging;
using Prometheus;
using Confluent.Kafka;

namespace sds_notificaciones
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "sds_notificaciones", Version = "v1" });
            });
            // Kafka
            services.AddSingleton<ConsumerConfig>(option => {
                return new ConsumerConfig
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
            });
            services.AddHostedService<KafkaConsumerHandler>();

            services.AddScoped<INotificacionService, NotificacionServiceImpl>();
            services.AddScoped<ITemplateService, TemplateServiceImpl>();
            services.AddScoped<IMailRepository, MailRepositoryImpl>();
            services.AddScoped<IMailClient, MailClientSendGrid>();
            
            // Mysql configuration
            var dbHost = Environment.GetEnvironmentVariable("DS_HOSTNAME");
            var dbPort = Environment.GetEnvironmentVariable("DS_PORT");
            var dbName = Environment.GetEnvironmentVariable("DS_DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DS_USERNAME");
            var dbPass = Environment.GetEnvironmentVariable("DS_PASSWORD");
            var connectionString = "server=" + dbHost + ";port=" + dbPort + ";user=" + dbUser + ";password=" + dbPass + ";database=" + dbName;

            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<DbContextImpl>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion)
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    //.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "sds_notificaciones v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/q/health");
                endpoints.MapMetrics();
            });
        }
    }
}
