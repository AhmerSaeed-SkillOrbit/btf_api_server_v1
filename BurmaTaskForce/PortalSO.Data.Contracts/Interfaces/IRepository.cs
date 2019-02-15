using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.Base;

namespace Btf.Data.Contracts.Interfaces
{
    /**
     * Repository Design Pattern -- Provides an abstraction over DbContext
     * Repository interface specification
     * Define minimum repository functionality
     * */
    public interface IRepository<T> where T : BaseEntity
    {
        /**
         * Get all records from the table
         * */
        IQueryable<T> GetAll();

        /**
         * Get all record from the table
         * Including linked entitites -- each provided as a parameter
         * */
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        /**
         * Get one row from the table based on Id
         * It's rapper around Find(id) method of DbSet 
         * */
        T GetById(int id);

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);

        /**
         * Add an entity and related entities to DbSet
         * Does not change anything in db until SaveChanges is called
         * */
        void Add(T entity);
        
        /**
         * Update an entity and related entities to DbSet
         * Does not change anything in db until SaveChanges is called
         * */
        void Update(T entity);

        /**
         * Delete an entity and related entities to DbSet
         * Does not change anything in db until SaveChanges is called
         * */
        void Delete(T entity);

        /**
         * Delete an entity and related entities to DbSet
         * Does not change anything in db until SaveChanges is called
         * */
        void Delete(int id);

        /**
         * Returns associated unit of work object
         * Which is responsible to run all changes in one transaction
         * */
         IUnitOfWork<TContext> GetUow<TContext>() where TContext : IDbContext;
    }
}
