using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Model.Base;
using Btf.Data.Model.User;
using Btf.Services.EmailService;
using Btf.Utilities.Exceptions;
using Btf.Utilities.Hash;
using Btf.Utilities.SixDigitKey;
using Btf.Utilities.UserSession;

namespace Btf.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserSession userSession;
        private IUserRepository userRepo;
        private IEmailService emaiService;
        private ISixDigitKeyProvider sixDigitKeyPrpvider;
        private IPasswordHash hashUtility;


        public UserService(IUserRepository userRepo, IEmailService emaiService, IUserSession userSession, ISixDigitKeyProvider sixDigitKeyPrpvider, IPasswordHash hashUtility)
        {
            this.userSession = userSession;
            this.userRepo = userRepo;
            this.emaiService = emaiService;
            this.sixDigitKeyPrpvider = sixDigitKeyPrpvider;
            this.hashUtility = hashUtility;
        }

        public User LoginUser => userSession.LoginUser;

        //register a new user completely without verfication
        //and without specialist information
        public async Task<int> RegisterUser(User newUser)
        {
            //Check if the user email is available
            var isEmailAvailable = await userRepo.IsEmailAvailable(newUser.Email);
            if (!isEmailAvailable)
                throw new BadRequestException("Email is not available");
            else if (String.IsNullOrEmpty(newUser.Email))
                throw new BadRequestException("Email address is required");
            else if (String.IsNullOrEmpty(newUser.FirstName))
                throw new BadRequestException("First name is required");
            else if (String.IsNullOrEmpty(newUser.LastName))
                throw new BadRequestException("Last name is required");

            var userStatus = await userRepo.GetUserStatusInfo("Completed");
            if (userStatus == null)
                throw new BadRequestException("User status Completed not configured.");

            newUser.UserGUID = Guid.NewGuid().ToString();
            //userRepo.Add(newUser);

            var defaultRole = await userRepo.GetUserRoleWithoutPermissions("Default");
            if (defaultRole == null)
                throw new BadRequestException("Default user role is not define");
            var userAccess = new UserAccess();
            userAccess.CreatedOn = userAccess.UpdatedOn = DateTime.UtcNow;
            userAccess.CreatedBy = userAccess.UpdatedBy = 1;
            userAccess.IsActive = true;
            userAccess.UserRoleId = defaultRole.Id;

            newUser.UserAccess.Add(userAccess);

            newUser.Password = "V33medSO!";

            userRepo.Add(newUser);

            userRepo.GetUow<UserContext>().Commit();

            return newUser.Id;

        }

       

        public async Task<int> AddUserAsync(User newUser)
        {
            //Check if the user email is available
            var isEmailAvailable = userRepo.IsEmailAvailable(newUser.Email).Result;
            if (!isEmailAvailable)
                throw new BadRequestException("Email is not available");
            else if (String.IsNullOrEmpty(newUser.Email))
                throw new BadRequestException("Email address is required");
            else if (String.IsNullOrEmpty(newUser.FirstName))
                throw new BadRequestException("First name is required");
            else if (String.IsNullOrEmpty(newUser.LastName))
                throw new BadRequestException("Last name is required");
            
            
            newUser.UserGUID = Guid.NewGuid().ToString();

            var defaultRole = await userRepo.GetUserRoleWithoutPermissions("Default");
            if (defaultRole == null)
                throw new BadRequestException("Default user role is not define");
            var userAccess = new UserAccess();
            userAccess.CreatedOn = userAccess.UpdatedOn = DateTime.UtcNow;
            userAccess.CreatedBy = userAccess.UpdatedBy = 1;
            userAccess.IsActive = true;
            userAccess.UserRoleId = defaultRole.Id;

            newUser.UserAccess.Add(userAccess);

            userRepo.Add(newUser);
            userRepo.GetUow<UserContext>().Commit();
            

            return newUser.Id;
        }

        public async Task<User> CheckLogin(string email, string password)
        {
            return await userRepo.CheckLogin(email, password);
        }

        public void CompleteRegistration(User entity, int userId)
        {
            UserStatus userStatus = userRepo.GetUserStatusInfo("Completed").Result;

            if (userStatus == null)
                throw new BadRequestException("User status Completed or PendingForApproval is not configured.");

            var storedUser = userRepo.GetUserInfoWithoutState(entity.Id).Result;
            
            if (storedUser == null)
                throw new BadRequestException("User id does not exist.");

            if (storedUser.Id != userId)
                throw new BadRequestException("Invalid user id.");
            

            //var defaultRole = userRepo.GetUserRoleWithoutPermissions("Default").Result;
            //if (defaultRole == null)
            //    throw new BadRequestException("Default user role is not define");
            //var userAccess = new UserAccess();
            //userAccess.CreatedOn = userAccess.UpdatedOn = DateTime.UtcNow;
            //userAccess.CreatedBy = userAccess.UpdatedBy = 1;
            //userAccess.IsActive = true;
            //userAccess.UserRoleId = defaultRole.Id;
            //userAccess.UserId = storedUser.Id;

            //storedUser.UserAccess.Add(userAccess);

            storedUser.FirstName = entity.FirstName;
            storedUser.LastName = entity.LastName;
            storedUser.MobileNumber = entity.MobileNumber;
            storedUser.Address = entity.Address;
            storedUser.Address1 = entity.Address1;

            storedUser.ZipCode = entity.ZipCode;
            storedUser.SecretAnswer1 = entity.SecretAnswer1;
            storedUser.SecretAnswer2 = entity.SecretAnswer2;
            storedUser.SecretQuestion1 = entity.SecretQuestion1;
            storedUser.SecretQuestion2 = entity.SecretQuestion2;
            storedUser.UpdatedOn = DateTime.UtcNow;
            storedUser.UpdatedBy = 0;

            userRepo.Update(storedUser);
            userRepo.GetUow<UserContext>().Commit();

            return;
        }

        public async Task<User> GetUserInfo(int userId)
        {
            var user = await userRepo.GetUserInfo(userId);
            return user;
        }

        public async Task<User> GetByUserGuid(string userGuid)
        {
            return await userRepo.GetByUserGuid(userGuid);
        }

        public async Task UpdateUser(User entity)
        {
            var storedUser = userRepo.GetUserInfoWithoutState(entity.Id).Result;
            if (storedUser == null)
                throw new BadRequestException("User id does not exist.");
          
            storedUser.FirstName = entity.FirstName;
            storedUser.LastName = entity.LastName;
            storedUser.MobileNumber = entity.MobileNumber;
            storedUser.Address = entity.Address;
            storedUser.Address1 = entity.Address1;
            storedUser.ZipCode = entity.ZipCode;
            if(!string.IsNullOrWhiteSpace(entity.SecretAnswer1) )
            {
                storedUser.SecretAnswer1 = entity.SecretAnswer1;
                storedUser.SecretQuestion1 = entity.SecretQuestion1;
            }

            if (!string.IsNullOrWhiteSpace(entity.SecretAnswer2))
            {
            storedUser.SecretAnswer2 = entity.SecretAnswer2;
            storedUser.SecretQuestion2 = entity.SecretQuestion2;
            }
            storedUser.UpdatedOn = DateTime.UtcNow;
            storedUser.UpdatedBy = 0;

            userRepo.Update(storedUser);

            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task<User> VerifyKey(string key, int userId)
        {
            var keyInfo = await userRepo.GetKeyInfo(key);
            if (keyInfo == null)
                throw new BadRequestException("Key does not exist.");

            if (keyInfo.UserId != userId)
                throw new BadRequestException("User id does not match with the key.");

            keyInfo.IsUsed = true;

            var userInfo = await userRepo.GetUserInfoWithoutState(keyInfo.UserId);

            if (userInfo == null)
                throw new BadRequestException("User does not exist.");

            var userStatus = await userRepo.GetUserStatusInfo("Verified");
            if (userStatus == null)
                throw new BadRequestException("User status Verified is not configured.");

            userRepo.Update(userInfo);

            userRepo.GetUow<UserContext>().Commit();

            return userInfo;

        }

        public async Task<bool> IsEmailAvailable(string Email)
        {
            var result = await userRepo.IsEmailAvailable(Email);
            return result;
        }

        public async Task<List<UserAccess>> GetUserAccess(int userId)
        {
            return await userRepo.GetUserAccess(userId);
        }

        public async Task<List<UserAccess>> GetUserAccessWithoutPermissions(int userId)
        {
            return await userRepo.GetUserAccessWithoutPermissions(userId);
        }

        public async Task<int> GetAllUsersCount()
        {
            return await userRepo.GetAllUsersCount();
        }

        public async Task<int> GetUsersCount(string status, string filter,int userId)
        {
            status = status.ToLower();
            bool activeOnly = status == "active" ? true : false;
            bool inactiveOnly = status == "inactive" ? true : false;

            return await userRepo.GetUsersCount(filter, activeOnly, inactiveOnly,userId);
        }

        public async Task<List<User>> GetUsers(int pageIndex, int pageSize, string filter, bool activeOnly, bool inactiveOnly,int userId)
        {
            return await userRepo.GetUsers(pageIndex, pageSize, filter, activeOnly, inactiveOnly, userId);
        }

        public async Task UpdateActiveStatus(int userId, bool isActive)
        {

            User user = null;
            user = await userRepo.GetUserWithoutStateAndStatus(userId);

            if (user == null)
                throw new BadRequestException("User id : " + userId.ToString() + " does not exist.");

            if (user.IsActive == isActive) return;

            user.IsActive = isActive;
            user.UpdatedOn = DateTime.UtcNow;
            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task AddUserAccess(UserAccess userAccess)
        {
            //check if user already have access
            var access = await userRepo.GetUserAccess(userAccess.UserId, userAccess.UserRoleId);
            if (access != null) return; //already exist

            userAccess.UpdatedOn = userAccess.CreatedOn = DateTime.UtcNow;
            userAccess.IsActive = true;

            userRepo.AddAccess(userAccess);

            userRepo.GetUow<UserContext>().Commit();            
        }

        public async Task RemoveUserAccess(UserAccess userAccess)
        {
            var access = await userRepo.GetUserAccessByAccessId(userAccess.Id);

            if (access == null)
                throw new BadRequestException("User access with id : " + userAccess.Id + " does not exist.");

            access.IsActive = false;
            access.UpdatedOn = DateTime.UtcNow;

            userRepo.UpdateAccess(access);

            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task<List<UserRole>> GetUserRoles()
        {
            return await userRepo.GetUserRoles();
        }

        public async Task UpdateUserAccess(List<UserAccess> accessList)
        {
            var added = accessList.Where(a => a.IsActive && a.Id == 0).ToList();

            foreach (var access in added)
            {
                access.CreatedOn = access.UpdatedOn = DateTime.UtcNow;
                userRepo.AddAccess(access);
            }

            var deleted = accessList.Where(a => !a.IsActive && a.Id != 0).ToList();

            foreach (var access in deleted)
            {
                var original = await userRepo.GetUserAccess(access.UserId, access.UserRoleId);
                if(original != null)
                {
                    original.IsActive = false;
                    original.UpdatedOn = DateTime.UtcNow;
                    userRepo.UpdateAccess(original);
                }
            }

            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task<List<Permission>> GetPermissions(string filter, int pageIndex, int pageSize)
        {
            return await userRepo.GetPermissions(filter, pageIndex, pageSize);
        }
        public async Task<int> GetPermissionsCount(string filter)
        {
            return await userRepo.GetPermissionsCount(filter);
        }

        public async Task<int> GetPermissionsCount(int roleId, string filter)
        {
            return await userRepo.GetPermissionsCount(roleId, filter);
        }
        public async Task<List<UserRolePermission>> GetPermissions(int roleId, string filter, int pageIndex, int pageSize)
        {
            return await userRepo.GetPermissions(roleId, filter, pageIndex, pageSize);
        }

        public async Task<List<Permission>> GetPermissionDependencies(int permissionId)
        {
            return await userRepo.GetPermissionDependencies(permissionId);
        }

        public async Task<bool> IsRoleNameAvailable(string roleName, int roleId)
        {
            return await userRepo.RoleNameAvailable(roleName, roleId);
        }

        public async Task AddRole(UserRole role)
        {
            //check if the role name already exist
            var available = await userRepo.RoleNameAvailable(role.Role);
            if (!available)
                throw new BadRequestException("Role name already exist.");

            role.IsActive = true;
            role.UpdatedOn = role.CreatedOn = DateTime.UtcNow;

                       
            foreach (var p in role.Permissions)
            {
                p.CreatedOn = p.UpdatedOn = DateTime.UtcNow;
                p.IsActive = true;
            }

            userRepo.AddRole(role);

            userRepo.GetUow<UserContext>().Commit();

            
        }

        public async Task UpdateRole(UserRole role)
        {
            if (String.IsNullOrWhiteSpace(role.Role))
                throw new BadRequestException("Role name can't be null");

            //check if the role name already exist
            var available = await userRepo.RoleNameAvailable(role.Role, role.Id);
            if (!available)
                throw new BadRequestException("Role name already exist.");

            var addedPermissions = role.Permissions.Where(p => p.IsActive && p.Id == 0).ToList();
            var deletedPermissions = role.Permissions.Where(p => !p.IsActive && p.Id != 0).ToList();

            var oldRole = await userRepo.GetUserRole(role.Id);

            if (oldRole == null)
                throw new BadRequestException("Role id:" + role.Id + " does not exist.");

            oldRole.Role = role.Role;

            foreach (var permission in addedPermissions)
            {
                var pexist = oldRole.Permissions.Any(p => p.IsActive && p.PermissionId == permission.PermissionId);
                if (pexist) continue;

                permission.UpdatedOn = permission.CreatedOn = DateTime.UtcNow;
                oldRole.Permissions.Add(permission);
            }

            foreach (var permission in deletedPermissions)
            {
                var oldPermission = oldRole.Permissions.FirstOrDefault(P => P.IsActive && P.Id == permission.Id);
                if (oldPermission == null) continue;

                oldPermission.IsActive = false;
                oldPermission.UpdatedOn = DateTime.UtcNow;
            }

            userRepo.UpdateRole(oldRole);

            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await userRepo.CheckPassword(userId, currentPassword);
            if (user == null) throw new BadRequestException("Invalid Password");

            user.Password = hashUtility.GetHash(newPassword);
            userRepo.Update(user);
            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task<List<string>> GetSecurityQuestions(string emailAddress)
        {
            return await userRepo.GetSecurityQuestions(emailAddress);
        }

        public async Task<bool> VerifySecurityAnswers(string emailAddress, string answer1, string answer2)
        {
            var user = await userRepo.VerifySecurityAnswers(emailAddress, answer1, answer2);
            if (user == null) return false;

            //generate a new verification key
            var newKey = sixDigitKeyPrpvider.GenerateNewKey();
            UserVerificationKey newUserKey = new UserVerificationKey();
            newUserKey.CreatedOn = newUserKey.UpdatedOn = DateTime.UtcNow;
            newUserKey.IsActive = true;
            newUserKey.VerificationKey = newKey;
            newUserKey.UserId = user.Id;
            newUserKey.IsUsed = false;
            newUserKey.ExpiresOn = DateTime.UtcNow.AddDays(1);

            userRepo.AddUserVerificationKey(newUserKey);
            userRepo.GetUow<UserContext>().Commit();

            //email new key to user
            string[] emailData = new string[] { user.FirstName, newKey };
            await emaiService.Send(user.Email, "VeeMed Change Password", emailData,isEmailVerification: false);

            return true;
        }

        public async Task ResetPassword(int userId, string userType)
        {
            var user = await userRepo.GetUserInfo(userId);
            if (user == null) throw new BadRequestException("Invalid user Id: " + userId.ToString());

            //generate a new verification key
            var newKey = sixDigitKeyPrpvider.GenerateNewKey();
            UserVerificationKey newUserKey = new UserVerificationKey();
            newUserKey.CreatedOn = newUserKey.UpdatedOn = DateTime.UtcNow;
            newUserKey.IsActive = true;
            newUserKey.VerificationKey = newKey;
            newUserKey.UserId = user.Id;
            newUserKey.IsUsed = false;
            newUserKey.ExpiresOn = DateTime.UtcNow.AddDays(1);

            userRepo.AddUserVerificationKey(newUserKey);
            userRepo.GetUow<UserContext>().Commit();

            //email new key to user
            string[] emailData = new string[] { user.FirstName, newKey };
            await emaiService.Send(user.Email, "VeeMed Reset Password", emailData, userType, false);
        }

        public async Task VerifyAndChangePassword(string verificationKey, string newPassword, string question1, string question2, string answer1 , string answer2)
        {
            //check the key 
            var keyInfo = await userRepo.GetKeyInfo(verificationKey);
            if (keyInfo == null)
                throw new BadRequestException("Key does not exist.");

            keyInfo.IsUsed = true;

            var userInfo = await userRepo.GetUserInfoWithoutState(keyInfo.UserId);

            if (userInfo == null)
                throw new BadRequestException("User does not exist.");

            userInfo.Password = hashUtility.GetHash(newPassword);
            userInfo.UpdatedOn = DateTime.UtcNow;
            //update secrete questions 
            if (string.IsNullOrWhiteSpace(question1) || string.IsNullOrWhiteSpace(question2) || string.IsNullOrWhiteSpace(answer1) || string.IsNullOrWhiteSpace(answer2))
            {
                throw new BadRequestException("Secret questions and answers cannot be empty.");
            }
            userInfo.SecretQuestion1 = question1;
            userInfo.SecretQuestion2 = question2;
            userInfo.SecretAnswer1 = answer1;
            userInfo.SecretAnswer2 = answer2;

            //make user a system user 
            //userInfo.SystemId = 1;

            userRepo.Update(userInfo);

            userRepo.GetUow<UserContext>().Commit();
        }
        public async Task UpdateSecretQuestionsAnswers(int userId, Dictionary<string, string> qanswers)
        {
            var user = await userRepo.GetUserInfoWithoutState(userId);
            user.SecretQuestion1 = qanswers.Keys.ElementAt(0);
            user.SecretQuestion2 = qanswers.Keys.ElementAt(1);
            user.SecretAnswer1 = qanswers.Values.ElementAt(0);
            user.SecretAnswer2 = qanswers.Values.ElementAt(1);
            userRepo.Update(user);
            userRepo.GetUow<UserContext>().Commit();
        }

        public Task AddPermission(Permission permission)
        {
            userRepo.AddPermission(permission);
            userRepo.GetUow<UserContext>().Commit();
            return Task.CompletedTask;
        }

        public async Task<List<KeyValuePair<string, string>>> GetUsersNamesByGUIDAsync(List<string> gUIDs)
        {
            return await userRepo.GetUsersNamesByGUIDAsync(gUIDs);
        }

        public async Task<List<Permission>> GetPermissionDependenciesAsync(List<int> permissionIds)
        {
            return await userRepo.GetPermissionDependenciesAsync(permissionIds);
        }

        public void Detach(IEntity entity)
        {
            userRepo.Detach(entity);
        }

        public async Task VerifyAndChangePassword(string verificationKey, string newPassword)
        {
            var keyInfo = await userRepo.GetKeyInfo(verificationKey);
            if (keyInfo == null)
                throw new BadRequestException("Key does not exist.");

            keyInfo.IsUsed = true;

            var userInfo = await userRepo.GetUserInfoWithoutState(keyInfo.UserId);

            if (userInfo == null)
                throw new BadRequestException("User does not exist.");

            userInfo.Password = hashUtility.GetHash(newPassword);
            userInfo.UpdatedOn = DateTime.UtcNow;
            userRepo.Update(userInfo);

            userRepo.GetUow<UserContext>().Commit();
        }

        public async Task<bool> CheckKartUserAsync(int userId)
        {
            List<UserRole> roles = await userRepo.GetUserRolesAsync(userId);
            return roles.Any(r => r.Role.ToLower() == "kart");
        }

        public async Task ChangeUserPasswordAsync(int userId, string password)
        {
            var user = await userRepo.GetUserInfo(userId);
            user.Password = hashUtility.GetHash(password);
            user.UpdatedOn = DateTime.UtcNow;
            userRepo.Update(user);
            userRepo.GetUow<UserContext>().Commit();
        }
    }
}
