using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.Project;
using Btf.Data.Model.User;

namespace Btf.Services.ProjectService
{
    public interface IProjectService
    {
        Task<List<Project>> GetLastFiveProjects(int userId);
        Task<List<ProjectActivity>> GetActivities(int userId,int projectId, int pageIndex, int pageSize);
        Task<List<ProjectTask>> GetAllTasks(int projectId);
        Task AddActivity(ProjectActivity projectActivity, int userId);
        Task<List<Project>> GetAllProjects(int userId);
        Task<List<ProjectActivity>> GetAllActivities(int taskId);
        Task<int> GetActivitiesCount(int userId, int projectId);
        Task<List<User>> GetAllUsers();
        Task<List<ProjectTask>> GetAllTasksWithActivities(int projectID);
        Task<List<ProjectTask>> GetAllTasksWithActivitiesByUser(int userId);
        Task<List<ProjectActivity>> GetAllActivitiesForTaskByUser(int userId, int taskId);
    }
}
