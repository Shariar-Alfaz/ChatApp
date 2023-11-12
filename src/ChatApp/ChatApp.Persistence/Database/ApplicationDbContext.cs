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
            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
            modelBuilder.Entity<ApplicationUserClaim>(entity => entity.ToTable("UserClaims"));
            modelBuilder.Entity<ApplicationUserLogin>(entity => entity.ToTable("UserLogins"));
            modelBuilder.Entity<ApplicationUserToken>(entity => entity.ToTable("UserTokens"));

            modelBuilder.Entity<ApplicationRole>(entity => entity.ToTable("Roles"));
            modelBuilder.Entity<ApplicationRoleClaim>(entity => entity.ToTable("RoleClaims"));
            modelBuilder.Entity<ApplicationUserRole>(entity => entity.ToTable("UserRoles"));
        }
    }
}
