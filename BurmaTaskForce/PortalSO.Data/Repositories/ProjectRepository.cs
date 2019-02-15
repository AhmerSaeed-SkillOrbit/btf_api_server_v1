using Microsoft.EntityFrameworkCore;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Base;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Model.Project;
using Btf.Data.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btf.Data.Repositories
{
    public class ProjectRepository : BaseRepository<Project, ProjectContext>, IProjectRepository
    {
        private DbSet<User> Users;
        private DbSet<Project> Projects;
        private DbSet<Client> Clients;
        private DbSet<ProjectTask> ProjectTasks;
        private DbSet<ProjectActivity> ProjectActivities;
        private DbSet<ProjectTeam> ProjectTeams;

        public ProjectRepository(IUnitOfWork<ProjectContext> uow) : base(uow)
        {
            Projects = Set;
            //var projectContext = (ProjectContext)Context;
            var projectContext = (ProjectContext)uow.Context;
            Users = projectContext.Users;
            Projects = projectContext.Projects;
            Clients = projectContext.Clients;
            ProjectTasks = projectContext.ProjectTasks;
            ProjectActivities = projectContext.ProjectActivities;
            ProjectTeams = projectContext.ProjectTeams;
        }

        public Task AddProjectActivity(ProjectActivity projectActivity)
        {
            ProjectActivities.Add(projectActivity);
            return Task.CompletedTask;
        }

        public async Task<List<ProjectActivity>> GetActivities(int userId, int projectId, int pageIndex, int pageSize)
        {
           return await ProjectActivities
                .Include(pa=>pa.Task)
                .Where(pa => pa.IsActive && pa.Task.ProjectId == projectId && pa.UserId == userId)
                .OrderByDescending(pa => pa.CreatedOn)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetActivitiesCount(int userId, int projectId)
        {
            return await ProjectActivities
               .Where(pa => pa.IsActive && pa.Task.ProjectId == projectId && pa.UserId == userId)
               .CountAsync();
        }

        public async Task<List<ProjectActivity>> GetAllActivitiesByTaskId(int taskId)
        {
           return await ProjectActivities
                 .Where(pa => pa.IsActive && pa.TaskId == taskId)
                 .ToListAsync();
        }

        public async Task<List<ProjectActivity>> GetAllActivitiesForTaskByUser(int userId, int taskId)
        {
            return await ProjectActivities
                 .Where(pa => pa.IsActive && pa.TaskId == taskId && pa.UserId == userId)
                 .ToListAsync();
        }

        public async Task<List<Project>> GetAllProjects(int userId)
        {
            return await Projects
                .Where(p => p.ProjectTeams.Any(pt => pt.UserId == userId && pt.IsActive) && p.IsActive)
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetAllTasks(int projectId)
        {
            return await ProjectTasks
                .Where(pt => pt.ProjectId == projectId && pt.IsActive && pt.Project.IsActive)
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetAllTasksWithActivities(int projectID)
        {
            return await ProjectTasks
                .Include(p=>p.ProjectActivities)
               .Where(pt => pt.ProjectId == projectID && pt.IsActive && pt.Project.IsActive)
               .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetAllTasksWithActivitiesByUser(int userId)
        {
            return await ProjectTasks
                .Include(p => p.ProjectActivities)
               .Where(pt => pt.ProjectActivities.Any(pa=>pa.UserId == userId && pa.IsActive) && pt.IsActive && pt.Project.IsActive)
               .ToListAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await Users
                .ToListAsync();
        }

        public async Task<List<Project>> GetLastFiveProjects(int userId)
        {
            //await Projects
            //     .Include(p => p.ProjectTeams)
            //     .Where(p => p.ProjectTeams.Any(pt => pt.UserId == userId && pt.IsActive) && p.IsActive)
            //     .OrderByDescending(p=>p.ProjectTeams.Select(pt=>pt.CreatedOn))
            //     .ToListAsync();

            return await ProjectTeams
                 .Where(pt => pt.UserId == userId && pt.IsActive && pt.Project.IsActive)
                 .OrderByDescending(pt => pt.CreatedOn)
                 .Select(pt => pt.Project)
                 .Take(5)
                 .ToListAsync();
        }
    }
}
