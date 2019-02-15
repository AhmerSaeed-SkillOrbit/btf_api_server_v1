using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Contracts.Interfaces
{
    /**
     * Db context interface
     * Provide base specification for db context
     * */
    public interface IDbContext : IDisposable
    {
        /**
         * Main method to save all the changes to database in one batch
         * */
        int SaveChanges();

        /**
         * At least one set is required for DbContext to work.
         * Additonal Db Set are defined at implementation level.
         * */
        DbSet<T> Set<T>() where T : class;

        DbContext GetContext();
    }
}
