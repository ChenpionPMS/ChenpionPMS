using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChenpionPMS.Models
{
    public class DatabaseContext : IdentityDbContext<User, Role, String>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public DatabaseContext(
            DbContextOptions options,
            IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<RoleMenuItem> RoleMenuItem { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IModel Model)
                {
                    var now = DateTime.Now;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            Model.UpdatedAt = now;
                            break;
                        case EntityState.Added:
                            Model.CreatedAt = now;
                            Model.UpdatedAt = now;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            Model.DeletedAt = now;
                            break;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(p => p.Id)
                    .HasMaxLength(127);
                entity.Property(m => m.Email)
                    .HasMaxLength(127);
                entity.Property(m => m.NormalizedEmail)
                    .HasMaxLength(127);
                entity.Property(m => m.NormalizedUserName)
                    .HasMaxLength(127);
                entity.Property(m => m.UserName)
                    .HasMaxLength(127);
                entity.ToTable("User");
            });

            builder.Entity<Role>(entity =>
            {
                entity.Property(m => m.Id)
                    .HasMaxLength(127);
                entity.Property(m => m.Name)
                    .HasMaxLength(127);
                entity.Property(m => m.NormalizedName)
                    .HasMaxLength(127);
                entity.ToTable("Role");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.Property(m => m.Id)
                    .HasMaxLength(127);
                entity.Property(m => m.UserId)
                    .HasMaxLength(127);
                entity.ToTable("UserClaim");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.Property(m => m.Id)
                    .HasMaxLength(127);
                entity.Property(m => m.RoleId)
                    .HasMaxLength(127);
                entity.ToTable("RoleClaim");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                // Primary key
                entity.HasKey(m => new { m.LoginProvider, m.ProviderKey });
                entity.Property(m => m.LoginProvider)
                    .HasMaxLength(127);
                entity.Property(m => m.ProviderKey)
                    .HasMaxLength(127);
                entity.ToTable("UserLogin");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                // Primary key
                entity.HasKey(m => new { m.UserId, m.RoleId });
                entity.Property(m => m.UserId)
                    .HasMaxLength(127);
                entity.Property(m => m.RoleId)
                    .HasMaxLength(127);
                entity.ToTable("UserRole");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                // Composite primary key consisting of the UserId, LoginProvider and Name
                entity.HasKey(m => new { m.UserId, m.LoginProvider, m.Name });

                entity.Property(m => m.UserId)
                    .HasMaxLength(127);
                entity.Property(m => m.LoginProvider)
                    .HasMaxLength(127);
                entity.Property(m => m.Name)
                    .HasMaxLength(127);
                entity.ToTable("UserToken");
            });

            builder.Entity<MenuItem>();

            builder.Entity<RoleMenuItem>(entity =>
            {
                entity.HasKey(m => new { m.RoleId, m.MenuItemId });
                entity.HasOne(m => m.Role)
                    .WithMany(m => m.RoleMenuItems)
                    .HasForeignKey(m => m.RoleId);
            });

            #region Deleted QueryFilter
            const string DELETE_AT_PROPERTY_NAME = "DeletedAt";

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var haveDeleteAt = entityType.FindProperty(DELETE_AT_PROPERTY_NAME);
                if (haveDeleteAt == null)
                {
                    continue;
                }

                // Create the query filter
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType);

                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(DateTime?));
                var DeletedAtProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant(DELETE_AT_PROPERTY_NAME));

                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, DeletedAtProperty, Expression.Constant(null, typeof(DateTime?)));

                var lambda = Expression.Lambda(compareExpression, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
            #endregion
        }
    }
}