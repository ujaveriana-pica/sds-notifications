using Microsoft.EntityFrameworkCore;
using System.Reflection;
using sds.notificaciones.core.entities;

namespace sds.notificaciones.infraestructure.Context
{
    public class DbContextImpl : DbContext
    {
        public DbContextImpl(DbContextOptions<DbContextImpl> options) : base(options) { }

        public DbSet<Mail> Mail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mail>(entity => {
                entity.ToTable("mail");
            });
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}