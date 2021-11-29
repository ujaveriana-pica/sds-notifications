using sds.notificaciones.core.Interfaces;
using sds.notificaciones.core.entities;
using sds.notificaciones.infraestructure.Context;

namespace sds.notificaciones.infraestructure.repositories
{
    public class MailRepositoryImpl : IMailRepository 
    {
        protected readonly DbContextImpl dbContextImpl;
        
        public MailRepositoryImpl(DbContextImpl dbContextImpl) 
        {
            this.dbContextImpl = dbContextImpl;
        }
        public  void Save(Mail mail) 
        {
            dbContextImpl.Mail.Add(mail);
            dbContextImpl.SaveChanges();
        }
    }
}