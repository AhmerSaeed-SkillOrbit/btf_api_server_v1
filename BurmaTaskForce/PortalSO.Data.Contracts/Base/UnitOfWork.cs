using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Contracts.Interfaces;

namespace Btf.Data.Contracts.Base
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContext
    {
        private TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public TContext Context
        {
            get
            {
                return _context;
            }
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
