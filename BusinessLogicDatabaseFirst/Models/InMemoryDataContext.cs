using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class InMemoryDataContext : IDataContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            var property = this.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(IDbSet<TEntity>));

            if (property != null)
            {
                property.SetValue(this, new InMemoryDbSet<TEntity>());
                return property.GetValue(this) as InMemoryDbSet<TEntity>;
            }
      

            //TODO: Maybe better is throw exception to know that property doesn't exist.
            return null;
        }

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException(); 
        }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }
    }

    public class InMemoryDbSet<T> : DbSet<T> where T : class
    {
        ObservableCollection<T> data;
        IQueryable query;

        public InMemoryDbSet()
        {
            data = new ObservableCollection<T>();
            query = data.AsQueryable();
        }
    }
}
