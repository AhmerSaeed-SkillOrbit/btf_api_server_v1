using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Base;
using Btf.Data.Contracts.Interfaces;
using Btf.Utilities.Hash;
using Btf.Utilities.SixDigitKey;
using Btf.Data.Model.User;
using Btf.Data.Model.Base;

namespace Btf.Data.Repositories
{
    public class UserRepository : BaseRepository<User, UserContext>, IUserRepository
    {
        private IPasswordHash hashUtility;
        private ISixDigitKeyProvider sixDigitKeyPrpvider;
        private DbSet<User> Users;
        private DbSet<UserStatus> UserStatus;
        private DbSet<UserVerificationKey> VerificationKeys;
        private DbSet<UserAccess> UserAccess;
        private DbSet<UserRole> UserRoles;
        private DbSet<UserRolePermission> UserRolePermissions;
        private DbSet<Permission> Permissions;
        private DbSet<PermissionDependency> PermissionDependency;

        public UserRepository(IUnitOfWork<UserContext> uow, IPasswordHash hashUtility, ISixDigitKeyProvider sixDigitKeyPrpvider) : base(uow)
        {
            this.hashUtility = hashUtility;
            this.sixDigitKeyPrpvider = sixDigitKeyPrpvider;
            Users = Set;
            var userContext = (UserContext)Context;
            VerificationKeys = userContext.VerificationKeys;
            UserStatus = userContext.UserStatus;
            UserAccess = userContext.UserAccess;
            UserRoles = userContext.UserRoles;
            Permissions = userContext.Permissions;
            UserRolePermissions = userContext.UserRolePermissions;
            PermissionDependency = userContext.PermissionDependency;

        }

        public override void Add(User entity)
        {
            //hashed the password
            entity.Password = hashUtility.GetHash(entity.Password);
            entity.CreatedOn = entity.UpdatedOn = DateTime.UtcNow;
            entity.UpdatedBy = entity.UpdatedBy = 0;
            entity.IsActive = true;

            base.Add(entity);
        }

        public override void Update(User entity)
        {
            //get stored user entity 
            //var storedUser = GetById(entity.Id);
            //if (storedUser == null)
            //    throw new Exception("User id does not exist.");


            //if (entity.UserStatus != null && entity.UserStatusId != 0)
            //{
            //    storedUser.UserStatusId = entity.UserStatusId;
            //    storedUser.UserStatus = entity.UserStatus;
            //}

            base.Update(entity);
        }

        public async Task<User> CheckLogin(string email, string password)
        {
            var hashedPassword = hashUtility.GetHash(password);

            var user = await this.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword && u.IsActive == true);

            return (user);
        }

        public UserVerificationKey GenerateNewVerificationKey()
        {
            //generate a new key
            UserVerificationKey newKey = new UserVerificationKey();
            newKey.CreatedBy = newKey.UpdatedBy = 0;
            newKey.UpdatedOn = newKey.CreatedOn = DateTime.UtcNow;
            newKey.ExpiresOn = DateTime.UtcNow.AddDays(1);
            newKey.IsActive = true;
            newKey.VerificationKey = this.sixDigitKeyPrpvider.GenerateNewKey();
            newKey.IsUsed = false;

            return newKey;
        }

        public async Task<UserVerificationKey> GetKeyInfo(string key)
        {
            var keyInfo = await this.VerificationKeys.FirstOrDefaultAsync(k => k.VerificationKey == key && !k.IsUsed && k.IsActive);

            return keyInfo;
        }

        public async Task<UserStatus> GetUserStatusInfo(string status)
        {
            var statusInfo = await this.UserStatus.FirstOrDefaultAsync(s => s.Status == status && s.IsActive);

            return statusInfo;
        }

        public async Task<User> GetUserInfo(int userId)
        {
            return await Users
                .FirstOrDefaultAsync(u => u.IsActive && u.Id == userId);
        }

        public async Task<User> GetUserInfoWithoutState(int userId)
        {
            return await Users.FirstOrDefaultAsync(u => u.IsActive && u.Id == userId);
        }

        public async Task<User> GetUserWithoutStateAndStatus(int userId)
        {
            var user = await Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<User> GetByUserGuid(string userGuid)
        {
            var user = await Users
                .FirstOrDefaultAsync(u => u.UserGUID == userGuid && u.IsActive);

            if (user == null)
                user = await Users.FirstOrDefaultAsync(u => u.UserGUID == userGuid && u.IsActive);

            return user;
        }

        public async Task<List<UserAccess>> GetUserAccess(int userId)
        {
            var access = await UserAccess.Include(u => u.UserRole.Permissions).ThenInclude(p => p.Permission)
                                       .Where(u => u.UserId == userId && u.IsActive && u.UserRole.IsActive).AsNoTracking()
                                       .ToListAsync();
            return access;
        }

        public void Detach(IEntity entity)
        {
            var userContext = (UserContext)Context;
            userContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task<List<UserAccess>> GetUserAccessWithoutPermissions(int userId)
        {
            var access = await UserAccess.Include(u => u.UserRole)
                                      .Where(u => u.UserId == userId && u.IsActive && u.UserRole.IsActive)
                                      .ToListAsync();
            return access;
        }
       
        public void UpdateKey(UserVerificationKey key)
        {
            key.UpdatedOn = DateTime.UtcNow;
            VerificationKeys.Update(key);
        }

        public async Task<bool> IsEmailAvailable(string Email)
        {
            var emailCount = await Users.CountAsync(u => u.Email == Email);

            return (emailCount == 0);
        }

        public async Task<int> GetAllUsersCount()
        {
            return await Users.
                CountAsync();
        }

        public async Task<int> GetUsersCount(string filter, bool activeOnly, bool inactiveOnly, int userId)
        {
            string[] filters = filter.Split(' ', ',');
            string filterTrimmed = string.Join("", filters);
            if (String.IsNullOrEmpty(filter))
            {
                return await Users
                    .Where(u => (!activeOnly || u.IsActive) && (!inactiveOnly || !u.IsActive) && u.Id != userId
                    ).CountAsync();
            }

            return await Users
                                .Where(u => ((u.FirstName + u.LastName).Contains(filterTrimmed) || u.Email.Contains(filter))
                                      && (!activeOnly || u.IsActive) && (!inactiveOnly || !u.IsActive) && u.Id != userId
                                      ).CountAsync();
        }

        public async Task<List<User>> GetUsers(int pageIndex, int pageSize, string filter, bool activeOnly, bool inactiveOnly, int userId)
        {
            string[] filters = filter.Split(' ', ',');
            string filterTrimmed = string.Join("", filters);
            if (String.IsNullOrEmpty(filter))
            {
                return await Users
                    .Where(u => (!activeOnly || u.IsActive) && (!inactiveOnly || !u.IsActive) && u.Id != userId
                    )
                    .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            }

            return await Users
                .Where(u => ((u.FirstName + u.LastName).Contains(filterTrimmed) || u.Email.Contains(filter))
                                      && (!activeOnly || u.IsActive) && (!inactiveOnly || !u.IsActive) && u.Id != userId
                                      )
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToListAsync();
        }

        public async Task<UserAccess> GetUserAccessByAccessId(int userAccessId)
        {
            return await UserAccess.FirstOrDefaultAsync(u => u.IsActive && u.Id == userAccessId);
        }

        public async Task<UserAccess> GetUserAccess(int userId, int userRoleId)
        {
            return await UserAccess.FirstOrDefaultAsync(u => u.IsActive
                                    && u.UserId == userId && u.UserRoleId == userRoleId);
        }

        public void AddAccess(UserAccess userAccess)
        {
            UserAccess.Add(userAccess);
        }

        public void UpdateAccess(UserAccess userAccess)
        {
            UserAccess.Update(userAccess);
        }
        public async Task<List<UserRole>> GetUserRoles()
        {
            return await UserRoles.Where(r => r.IsActive).ToListAsync();
        }

        public async Task<UserRole> GetUserRole(int roleId)
        {
            return await UserRoles.Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.IsActive && r.Id == roleId);
        }

        public async Task<bool> RoleNameAvailable(string roleName, int roleId = 0)
        {
            var anyRecord = await UserRoles.AnyAsync(r => r.Role == roleName && r.IsActive && r.Id != roleId);

            return !anyRecord;
        }

        public async Task<UserRole> GetUserRoleWithoutPermissions(string roleName)
        {
            return await UserRoles.FirstOrDefaultAsync(r => r.IsActive && r.Role == roleName);
        }

        public async Task<int> GetPermissionsCount(string filter)
        {
            if (filter == "$0")
            {
                return await Permissions.CountAsync(p => p.IsActive && p.PermissionType != "API");
            }

            return await Permissions.CountAsync(p => (p.AccessUrl.Contains(filter)
                                                    || p.PermissionCode.Contains(filter)
                                                    || p.PermissionName.Contains(filter)
                                                    || p.PermissionType.Contains(filter))
                                                    && p.IsActive && p.PermissionType != "API");
        }

        public async Task<List<Permission>> GetPermissions(string filter, int pageIndex, int pageSize)
        {
            if (filter == "$0")
            {
                return await Permissions.Where(p => p.IsActive && p.PermissionType != "API")
                           .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            }

            return await Permissions.Where(p => (p.AccessUrl.Contains(filter)
                                                    || p.PermissionCode.Contains(filter)
                                                    || p.PermissionName.Contains(filter)
                                                    || p.PermissionType.Contains(filter))
                                                    && p.IsActive && p.PermissionType != "API")
                                                    .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> GetPermissionsCount(int roleId, string filter)
        {
            if (filter == "$0")
            {
                return await UserRolePermissions.CountAsync(r => r.Permission.IsActive
                                                && r.IsActive && r.UserRoleId == roleId && r.Permission.PermissionType != "API");
            }

            return await UserRolePermissions.CountAsync(r => (r.Permission.AccessUrl.Contains(filter)
                                                || r.Permission.PermissionCode.Contains(filter)
                                                || r.Permission.PermissionName.Contains(filter)
                                                || r.Permission.PermissionType.Contains(filter))
                                                && r.Permission.IsActive
                                                && r.IsActive && r.UserRoleId == roleId && r.Permission.PermissionType != "API");
        }

        public async Task<List<UserRolePermission>> GetPermissions(int roleId, string filter, int pageIndex, int pageSize)
        {
            if (filter == "$0")
            {
                return await UserRolePermissions.Include(r => r.Permission)
                                                .Where(r => r.IsActive && r.UserRoleId == roleId && r.Permission.PermissionType != "API")
                                                .Skip(pageIndex * pageSize).Take(pageSize)
                                                .ToListAsync();
            }

            return await UserRolePermissions.Include(r => r.Permission).Where(r => (r.Permission.AccessUrl.Contains(filter)
                                                || r.Permission.PermissionCode.Contains(filter)
                                                || r.Permission.PermissionName.Contains(filter)
                                                || r.Permission.PermissionType.Contains(filter))
                                                && r.Permission.IsActive
                                                && r.IsActive && r.UserRoleId == roleId && r.Permission.PermissionType != "API")
                                                .Skip(pageIndex * pageSize).Take(pageSize)
                                                .ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionDependencies(int permissionId)
        {
            return await PermissionDependency.Where(d => d.IsActive
                                                     && d.PermissionId == permissionId)
                                                    .Select(d => d.ChildPermission)
                                                    .ToListAsync();
        }

        public void AddRole(UserRole role)
        {
            UserRoles.Add(role);
        }

        public void UpdateRole(UserRole role)
        {
            UserRoles.Update(role);
        }

        public void AddRolePermission(UserRolePermission permmission)
        {
            UserRolePermissions.Add(permmission);
        }

        public void UpdateRolePermission(UserRolePermission permission)
        {
            UserRolePermissions.Update(permission);
        }

        public async Task<User> CheckPassword(int userId, string password)
        {
            var cPass = hashUtility.GetHash(password);

            //check if the current password is correct
            return await Users.FirstOrDefaultAsync(u => u.IsActive && u.Id == userId && u.Password == cPass);

        }

        public async Task<List<string>> GetSecurityQuestions(string emailAddress)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Email == emailAddress && u.IsActive);
            if (user == null) return new List<string>();

            var questions = new List<string>();
            questions.Add(user.SecretQuestion1);
            questions.Add(user.SecretQuestion2);

            return questions;
        }

        public async Task<User> VerifySecurityAnswers(string emailAddress, string answer1, string answer2)
        {
            return await Users.FirstOrDefaultAsync(u => u.Email == emailAddress
                                         && u.SecretAnswer1 == answer1 && u.SecretAnswer2 == answer2 && u.IsActive);
        }

        public void AddUserVerificationKey(UserVerificationKey newKey)
        {
            VerificationKeys.Add(newKey);
        }
        

        public void AddPermission(Permission permission)
        {
            permission.CreatedOn = permission.UpdatedOn = DateTime.UtcNow;
            permission.IsActive = true;
            Permissions.Add(permission);
        }

        public async Task<List<KeyValuePair<string, string>>> GetUsersNamesByGUIDAsync(List<string> gUIDs)
        {
            return await Users
                .Where(u => gUIDs.Any(g => g == u.UserGUID) && u.IsActive)
                .Select(u => new KeyValuePair<string, string>(u.UserGUID, u.FirstName + " " + u.LastName))
                .ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionDependenciesAsync(List<int> permissionIds)
        {
            return await PermissionDependency
                .Where(pd => permissionIds.Contains(pd.PermissionId) && pd.IsActive)
                .Select(pd => pd.ChildPermission).AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<UserRole>> GetUserRolesAsync(int userId)
        {
            return await UserAccess
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.UserRole)
                .ToListAsync();
        }
    }
}
