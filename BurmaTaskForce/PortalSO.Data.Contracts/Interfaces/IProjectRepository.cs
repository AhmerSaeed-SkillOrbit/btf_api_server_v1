using Btf.Data.Model.Project;
using Btf.Data.Model.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Btf.Data.Contracts.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetLastFiveProjects(int userId);
        Task<List<ProjectActivity>> GetActivities(int userId, int projectId, int pageIndex, int pageSize);
        Task<List<ProjectTask>> GetAllTasks(int projectId);
        Task AddProjectActivity(ProjectActivity projectActivity);
        Task<List<Project>> GetAllProjects(int userId);
        Task<List<ProjectActivity>> GetAllActivitiesByTaskId(int taskId);
        Task<int> GetActivitiesCount(int userId, int projectId);
        Task<List<User>> GetAllUsers();
        Task<List<ProjectTask>> GetAllTasksWithActivities(int projectID);
        Task<List<ProjectTask>> GetAllTasksWithActivitiesByUser(int userId);
        Task<List<ProjectActivity>> GetAllActivitiesForTaskByUser(int userId, int taskId);
    }
}
