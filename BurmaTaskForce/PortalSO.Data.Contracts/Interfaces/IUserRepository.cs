using System.Collections.Generic;
using System.Threading.Tasks;
using Btf.Data.Model.Base;
using Btf.Data.Model.User;

namespace Btf.Data.Contracts.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> CheckLogin(string email, string password);
        Task<UserVerificationKey> GetKeyInfo(string key);
        Task<User> CheckPassword(int userId, string password);
        Task<List<string>> GetSecurityQuestions(string emailAddress);
        Task<User> VerifySecurityAnswers(string emailAddress, string answer1, string answer2);
        void AddUserVerificationKey(UserVerificationKey newKey);
        UserVerificationKey GenerateNewVerificationKey();

        Task<UserStatus> GetUserStatusInfo(string status);

        Task<User> GetUserInfo(int userId);
        Task<User> GetUserInfoWithoutState(int userId);
        

        Task<User> GetByUserGuid(string userGuid);

        void UpdateKey(UserVerificationKey key);
        
        

        Task<bool> IsEmailAvailable(string Email);
        Task<List<UserAccess>> GetUserAccess(int userId);
        Task<List<UserAccess>> GetUserAccessWithoutPermissions(int userId);
        Task<int> GetAllUsersCount();
        Task<int> GetUsersCount(string filter, bool activeOnly, bool inactiveOnly,int userId);
        Task<List<User>> GetUsers(int pageIndex, int pageSize, string filter, bool activeOnly, bool inactiveOnly,int userId);
        void AddAccess(UserAccess userAccess);
        void UpdateAccess(UserAccess userAccess);
        Task<UserAccess> GetUserAccessByAccessId(int userAccessId);
        Task<UserAccess> GetUserAccess(int userId, int userRoleId);
        Task<User> GetUserWithoutStateAndStatus(int userId);
        Task<List<UserRole>> GetUserRoles();
        Task<UserRole> GetUserRole(int roleId);
        Task<bool> RoleNameAvailable(string roleName, int roleId = 0);
        Task<List<Permission>> GetPermissions(string filter, int pageIndex, int pageSize);
        Task<int> GetPermissionsCount(string filter);
        Task<int> GetPermissionsCount(int roleId, string filter);
        Task<List<UserRolePermission>> GetPermissions(int roleId, string filter, int pageIndex, int pageSize);
        Task<List<Permission>> GetPermissionDependencies(int permissionId);

        void AddRole(UserRole role);
        void UpdateRole(UserRole role);
        void AddRolePermission(UserRolePermission permmission);
        void UpdateRolePermission(UserRolePermission permission);

        Task<UserRole> GetUserRoleWithoutPermissions(string roleName);
        void AddPermission(Permission permission);
        Task<List<KeyValuePair<string, string>>> GetUsersNamesByGUIDAsync(List<string> gUIDs);
        Task<List<Permission>> GetPermissionDependenciesAsync(List<int> permissionIds);

        void Detach(IEntity entity);
        Task<List<UserRole>> GetUserRolesAsync(int userId);
    }
}
