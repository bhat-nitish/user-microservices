using Microsoft.EntityFrameworkCore;
using UsersService.Entities;
using UsersService.Helpers;

namespace UsersService.Context
{
    public class UsersServiceContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<IntegrationEvent> IntegrationEvents { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL(ConnectionStringHelper.GetConnectionString());

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<IntegrationEvent>(entity =>
            {
                entity.ToTable(name: "IntegrationEvents");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<UserNotification>(entity =>
            {
                entity.ToTable(name: "UserNotifications");
                entity.HasKey(e => e.Id);
            });
        }
    }
}
