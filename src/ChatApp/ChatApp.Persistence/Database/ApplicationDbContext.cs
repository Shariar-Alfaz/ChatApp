using ChatApp.Persistence.Database.Base;
using ChatApp.Persistence.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Persistence.Database
{
    public class ApplicationDbContext 
        : IdentityDbContext<
            ApplicationUser,
            ApplicationRole,
            Guid,
            ApplicationUserClaim,
            ApplicationUserRole,
            ApplicationUserLogin,
            ApplicationRoleClaim,
            ApplicationUserToken>,
            IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public ApplicationDbContext() { }

        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly))
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
