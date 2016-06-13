using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace BusinessLogic.IdentityStores
{
    using Models;
    public class RoleStore : IQueryableRoleStore<Role, int>
    {
        private readonly IDataContext db;

        public RoleStore(IDataContext db)
        {
            this.db = db;
        }

        //// IQueryableRoleStore<UserRole, TKey>

        public IQueryable<Role> Roles
        {
            get { return this.db.Roles; }
        }

        //// IRoleStore<UserRole, TKey>

        public virtual Task CreateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Roles.Add(role);
            return this.db.SaveChangesAsync();
        }

        public Task DeleteAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Roles.Remove(role);
            return this.db.SaveChangesAsync();
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            return this.db.Roles.FindAsync(new[] { roleId });
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return this.db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public Task UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Entry(role).State = EntityState.Modified;
            return this.db.SaveChangesAsync();
        }

        //// IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.db != null)
            {
                this.db.Dispose();
            }
        }
    }
}