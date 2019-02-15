using Microsoft.EntityFrameworkCore;
using Btf.Data.Contracts.Interfaces;

namespace Btf.Data.Contracts.Base
{
    public class BaseDbContext<T> : DbContext, IDbContext where T : DbContext
    {
        public BaseDbContext(DbContextOptions<T> options) : base(options)
        {
        }

        public DbContext GetContext()
        {
            return this;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
