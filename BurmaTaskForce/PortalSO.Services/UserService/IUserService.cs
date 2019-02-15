using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.Base;
using Btf.Data.Model.User;

namespace Btf.Services.UserService
{
    public interface IUserService
    {
        Task<int> RegisterUser(User newUser);

        User LoginUser { get; }
        Task<User> CheckLogin(string email, string password);
        Task<User> VerifyKey(string key, int userId);

        Task<User> GetUserInfo(int userId);
        Task<User> GetByUserGuid(string userGuid);
        Task<int> AddUserAsync(User newUser);
        Task UpdateUser(User updatedUser);

        void CompleteRegistration(User updatedUser, int userId);
        Task<bool> IsEmailAvailable(string Email);
        Task<List<UserAccess>> GetUserAccess(int userId);
        Task<List<UserAccess>> GetUserAccessWithoutPermissions(int userId);
        Task<int> GetAllUsersCount();
        Task<int> GetUsersCount(string status, string filter,int userId);
        Task<List<User>> GetUsers(int pageIndex, int pageSize, string filter, bool activeOnly, bool deactiveOnly,int userId);
        Task<List<Permission>> GetPermissionDependenciesAsync(List<int> permissionIds);
        Task UpdateActiveStatus(int userId, bool isActive);
        Task AddUserAccess(UserAccess userAccess);
        Task RemoveUserAccess(UserAccess userAccess);
        Task<List<UserRole>> GetUserRoles();
        Task UpdateUserAccess(List<UserAccess> accessList);

        Task<List<Permission>> GetPermissions(string filter, int pageIndex, int pageSize);
        Task<int> GetPermissionsCount(string filter);
        Task<int> GetPermissionsCount(int roleId, string filter);
        Task<List<UserRolePermission>> GetPermissions(int roleId, string filter, int pageIndex, int pageSize);
        Task<List<Permission>> GetPermissionDependencies(int permissionId);
        Task<bool> IsRoleNameAvailable(string roleName, int roleId);
        Task VerifyAndChangePassword(string verificationKey, string newPassword);
        Task AddRole(UserRole role);
        Task<List<KeyValuePair<string, string>>> GetUsersNamesByGUIDAsync(List<string> gUIDs);
        Task UpdateRole(UserRole role);
        Task ChangePassword(int userId, string currentPassword, string newPassword);
        Task<List<string>> GetSecurityQuestions(string emailAddress);
        Task<bool> VerifySecurityAnswers(string emailAddress, string answer1, string answer2);
        Task VerifyAndChangePassword(string verificationKey, string newPassword,string question1, string question2, string answer1, string answer2);
        Task ResetPassword(int userId, string userType);
        Task UpdateSecretQuestionsAnswers(int id, Dictionary<string, string> qanswers);
        Task AddPermission(Permission permission);
        void Detach(IEntity entity);
        Task<bool> CheckKartUserAsync(int userId);
        Task ChangeUserPasswordAsync(int userId, string password);
    }
}
