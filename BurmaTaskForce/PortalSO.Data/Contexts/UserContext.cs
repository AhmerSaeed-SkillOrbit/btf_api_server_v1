using Microsoft.EntityFrameworkCore;
using Btf.Data.Contracts.Base;
using Btf.Data.Model.User;

namespace Btf.Data.Contexts
{
    public class UserContext : BaseDbContext<UserContext>
    {
        public DbSet<User> Users;
        public DbSet<UserAccess> UserAccess { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        public DbSet<UserStatus> UserStatus { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionDependency> PermissionDependency { get; set; }
        public DbSet<UserVerificationKey> VerificationKeys { get; set; }
        
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        /**
         * Map relationship between different tables (entities)
         * */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserVerificationKey>().ToTable("UserVerificationKey");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserRolePermission>().ToTable("UserRolePermission");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
