using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Btf.Data.Contracts.Base;
using Btf.Data.Model.User;

namespace Btf.Data.Contexts
{
    public class LocationContext : BaseDbContext<LocationContext>
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public LocationContext(DbContextOptions<LocationContext> options) : base(options)
        {
        }

        /**
        * Map relationship between different tables (entities)
        * */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<State>().ToTable("State");
            modelBuilder.Entity<City>().ToTable("City");

            base.OnModelCreating(modelBuilder);
        }
    }
}
