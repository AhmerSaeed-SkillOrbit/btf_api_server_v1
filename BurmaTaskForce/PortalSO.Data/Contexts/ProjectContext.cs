using Microsoft.EntityFrameworkCore;
using Btf.Data.Contracts.Base;
using Btf.Data.Model.Project;
using Btf.Data.Model.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Contexts
{
    public class ProjectContext : BaseDbContext<ProjectContext>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectActivity> ProjectActivities { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<ProjectTask>().ToTable("ProjectTask");
            modelBuilder.Entity<ProjectActivity>().ToTable("ProjectActivity").HasOne<ProjectTask>(a=>a.Task);
            modelBuilder.Entity<ProjectTeam>().ToTable("ProjectTeam");
            modelBuilder.Entity<User>().ToTable("User");

            base.OnModelCreating(modelBuilder);
        }
    }
}
