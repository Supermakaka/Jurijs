using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

using Microsoft.AspNet.Identity.EntityFramework;

namespace BusinessLogic.Models
{
    public class DataContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>, IDataContext, IDisposable
    {
        public DataContext() : base("DataContext") { }

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }
        public IDbSet<UserLogin> UserLogins { get; set; }
        public IDbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Change Asp.Net Identity default table names
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
        }
    }

    public interface IDataContext
    {
        IDbSet<Company> Companies { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<UserRole> UserRoles { get; set; }
        IDbSet<UserLogin> UserLogins { get; set; }
        IDbSet<UserClaim> UserClaims { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}