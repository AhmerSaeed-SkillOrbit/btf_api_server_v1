using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Btf.Data.Contracts.Base;
using Btf.Data.Model.Log;

namespace Btf.Data.Contexts
{
    public class LogContext : BaseDbContext<LogContext>
    {
        public DbSet<LogRequest> LogRequests { get; set; }
        public DbSet<LogMsg> LogMsgs { get; set; }

        public LogContext(DbContextOptions<LogContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogRequest>().ToTable("LogRequest");
            modelBuilder.Entity<LogMsg>().ToTable("LogMsg");

            base.OnModelCreating(modelBuilder);
        }
    }
}
