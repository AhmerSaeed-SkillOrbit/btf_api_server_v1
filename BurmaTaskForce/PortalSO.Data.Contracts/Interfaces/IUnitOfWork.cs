using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Contracts.Interfaces
{
    /**
     * Unit Of Work Design Pattern
     * Responsible to save all changes to database in one transaction
     * Different repositories can share same unit of work object for transation managment
     * */
    public interface IUnitOfWork<TContext> : IDisposable where TContext : IDbContext
    {
        /**
         * Commit all changes to database
         * */
        int Commit();

        /**
         * Returns associated db context
         * */
        TContext Context { get; }
    }
}
