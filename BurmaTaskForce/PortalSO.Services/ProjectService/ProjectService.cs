using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Model.Project;
using Btf.Data.Model.User;

namespace Btf.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task AddActivity(ProjectActivity projectActivity,int userId)
        {
            projectActivity.CreatedBy = 0;
            projectActivity.UpdatedBy = 0;
            projectActivity.CreatedOn = DateTime.UtcNow;
            projectActivity.UpdatedOn = DateTime.UtcNow;
            projectActivity.UserId = userId;
            projectActivity.IsActive = true;
            await projectRepository.AddProjectActivity(projectActivity);
            projectRepository.GetUow<ProjectContext>().Commit();
        }

        public async Task<List<ProjectActivity>> GetActivities(int userId, int projectId, int pageIndex, int pageSize)
        {
            return await projectRepository.GetActivities(userId, projectId, pageIndex, pageSize);
        }

        public async Task<int> GetActivitiesCount(int userId, int projectId)
        {
            return await projectRepository.GetActivitiesCount(userId,projectId);
        }

        public async Task<List<ProjectActivity>> GetAllActivities(int taskId)
        {
            return await projectRepository.GetAllActivitiesByTaskId(taskId);  
        }

        public async Task<List<ProjectActivity>> GetAllActivitiesForTaskByUser(int userId, int taskId)
        {
            return await projectRepository.GetAllActivitiesForTaskByUser(userId,taskId);
        }

        public async Task<List<Project>> GetAllProjects(int userId)
        {
           return await projectRepository.GetAllProjects(userId);
        }

        public async Task<List<ProjectTask>> GetAllTasks(int projectId)
        {
            return await projectRepository.GetAllTasks(projectId);
        }

        public async Task<List<ProjectTask>> GetAllTasksWithActivities(int projectID)
        {
            return await projectRepository.GetAllTasksWithActivities(projectID);
        }

        public async Task<List<ProjectTask>> GetAllTasksWithActivitiesByUser(int userId)
        {
            return await projectRepository.GetAllTasksWithActivitiesByUser(userId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await projectRepository.GetAllUsers();
        }
        
        public async Task<List<Project>> GetLastFiveProjects(int userId)
        {
           return await projectRepository.GetLastFiveProjects(userId);
        }
    }
}
