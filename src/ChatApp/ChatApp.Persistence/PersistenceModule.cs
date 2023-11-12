using Autofac;
using ChatApp.Persistence.Database;
using ChatApp.Persistence.Database.Base;

namespace ChatApp.Persistence
{
    public class PersistenceModule: Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public PersistenceModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterType<ApplicationDbContext>()
               .AsSelf()
               .WithParameter("connectionString", _connectionString)
               .WithParameter("migrationAssembly", _migrationAssembly)
               .InstancePerLifetimeScope();

            builder
                .RegisterType<ApplicationDbContext>()
                .As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
