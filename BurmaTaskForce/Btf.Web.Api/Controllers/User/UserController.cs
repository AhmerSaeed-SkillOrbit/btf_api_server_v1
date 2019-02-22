using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Btf.Data.Contracts.Interfaces;
using Btf.Services.UserService;
using Btf.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Btf.Web.Api.DTO;
using Btf.Data.Model.User;

namespace Btf.Web.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IUserService userService) : base(userService)
        {
        }

        #region User Registration

        [HttpPut]
        [Route("api/user/registration/verify")]
        public IActionResult Verify([FromBody] UserVerificationKey VerificationKey)
        {
            LoadTokenInfo();
            var key = VerificationKey.VerificationKey;
            var user = UserService.VerifyKey(key, LoginUser.Id);
            return Ok(new UserDto(user.Result));
        }

        [HttpPut]
        [Route("api/user/registration/complete")]
        public IActionResult CompleteRegistratoin([FromBody] Btf.Data.Model.User.User updatedUser)
        {
            LoadTokenInfo();
            UserService.CompleteRegistration(updatedUser, LoginUser.Id);
            return Ok("User registration has been completed.");
        }

        [HttpPost]
        [Route("api/user/register")]
        public async Task<IActionResult> RegisterUser([FromBody] Btf.Data.Model.User.User newUser)
        {
            var userId = await UserService.RegisterUser(newUser);
            return Ok(userId);
        }

        #endregion

        #region User & User Access

        [HttpGet]
        [Route("api/user/info")]
        public async Task<IActionResult> Get()
        {
            return Ok(new UserDto(LoginUser));

        }

        [HttpGet]
        [Route("api/user/info/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await UserService.GetUserInfo(userId);
            return Ok(new UserDto(user));
        }

        [HttpGet]
        [Route("api/endpoint/info/{endpointSerial}")]
        public async Task<IActionResult> GetEndPointInfo(string endpointSerial)
        {
            return Ok(new UserDto(LoginUser));
        }

        [HttpGet]
        [Route("api/user/roles")]
        public async Task<IActionResult> GetUserRoles()
        {

            LoadTokenInfo();
            var access = await UserService.GetUserAccess(LoginUser.Id);
            var rolesDto = access.Select(a => new UserRoleDto(a.UserRole)).ToList();
            return Ok(rolesDto);

        }

        [HttpGet]
        [Route("api/user/roles/{type}")]
        public async Task<IActionResult> GetUserRoles(string type)
        {

            type = type.ToUpper();
            if (type != PermissionTypes.WEB.ToString() && type != PermissionTypes.API.ToString()
                && type != PermissionTypes.PORTAL.ToString() && type != PermissionTypes.IOS.ToString())
                return BadRequest("Invalid permission type");

            LoadTokenInfo();
            var access = await UserService.GetUserAccess(LoginUser.Id);
            var rolesDto = access.Select(a => new UserRoleDto(a.UserRole, type)).ToList();
            return Ok(rolesDto);
        }

        [HttpGet]
        [Route("api/user/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {

            LoadTokenInfo();
            var access = await UserService.GetUserAccessWithoutPermissions(userId);
            var rolesDto = access.Select(a => new UserRoleDto(a.UserRole, a.Id, false)).ToList();
            return Ok(rolesDto);
        }


        [HttpGet]
        [Route("api/user/count")]
        public async Task<IActionResult> GetAllUsersCount()
        {
            return Ok(await UserService.GetAllUsersCount());
        }

        [HttpGet]
        [Route("api/user/count/{status}/{filter}")]
        public async Task<IActionResult> GetUsersCount(string status, string filter)
        {
            return Ok(await UserService.GetUsersCount(status, filter, LoginUser.Id));
        }

        [HttpGet]
        [Route("api/user/count/{status}")]
        public async Task<IActionResult> GetUsersCount(string status)
        {
            return Ok(await UserService.GetUsersCount(status, "", LoginUser.Id));
        }

        [HttpGet]
        [Route("api/user/{status}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUsers(string status, int pageIndex, int pageSize)
        {
            status = status.ToLower();
            bool activeOnly = status == "active" ? true : false;
            bool inactiveOnly = status == "inactive" ? true : false;

            var users = await UserService.GetUsers(pageIndex, pageSize, "", activeOnly, inactiveOnly, LoginUser.Id);

            var usersDto = users.Select(u => new UserDto(u));

            return Ok(usersDto);
        }

        [HttpGet]
        [Route("api/user/{status}/{filter}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUsers(string status, string filter, int pageIndex, int pageSize)
        {
            status = status.ToLower();
            bool activeOnly = status == "active" ? true : false;
            bool inactiveOnly = status == "inactive" ? true : false;

            var users = await UserService.GetUsers(pageIndex, pageSize, filter, activeOnly, inactiveOnly, LoginUser.Id);

            var usersDto = users.Select(u => new UserDto(u));

            return Ok(usersDto);
        }

        [HttpPut]
        [Route("api/user/profile/update")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            await UserService.UpdateUser(user);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/update")]
        public async Task<IActionResult> UpdateLoginUser([FromBody] User user)
        {
            LoadTokenInfo();
            if (user.Id != LoginUser.Id)
                return BadRequest("Invalid user id");
            else if (user.Email != LoginUser.Email)
                return BadRequest("Invalid email");

            await UserService.UpdateUser(user);
            var response = true;
            return Ok(response);
        }

        [HttpPut]
        [Route("api/user/secretanswer/update")]
        public async Task<IActionResult> UpdateSecretAnswer([FromBody] SecretAnswerUpdateDto qAUpdateDto)
        {
            LoadTokenInfo();
            var qanswers = new Dictionary<string, string>();
            qanswers.Add(qAUpdateDto.SecretQuestion1, qAUpdateDto.SecretAnswer1);
            qanswers.Add(qAUpdateDto.SecretQuestion2, qAUpdateDto.SecretAnswer2);
            await UserService.UpdateSecretQuestionsAnswers(LoginUser.Id, qanswers);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/activate/{userId}")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            LoadTokenInfo();
            await UserService.UpdateActiveStatus(userId, true);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/deactivate/{userId}")]
        public async Task<IActionResult> DeActivateUser(int userId)
        {
            LoadTokenInfo();
            await UserService.UpdateActiveStatus(userId, false);
            return Ok();
        }

        [HttpPost]
        [Route("api/user/access/add")]
        public async Task<IActionResult> AddRole([FromBody] UserAccess newAccess)
        {
            LoadTokenInfo();
            await UserService.AddUserAccess(newAccess);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/access/remove")]
        public async Task<IActionResult> RemoveRole([FromBody] UserAccess access)
        {
            LoadTokenInfo();
            await UserService.RemoveUserAccess(access);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/access/update")]
        public async Task<IActionResult> UpdateUserAccess([FromBody] List<UserAccess> accessList)
        {
            LoadTokenInfo();
            await UserService.UpdateUserAccess(accessList);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/password/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changedPassword)
        {
            await UserService.ChangePassword(LoginUser.Id, changedPassword.CurrentPassword, changedPassword.NewPassword);
            return Ok();
        }

        [HttpPut]
        [Route("api/user/{userId}/password/reset/{userType}")]
        public async Task<IActionResult> ResetPassword(int userId, string userType)
        {
            if (!(userType.ToLower() == "veedoc" || userType.ToLower() == "veeportal"))
            {
                return BadRequest("User type is not supported.");
            }
            await UserService.ResetPassword(userId, userType);
            return Ok();
        }

        [HttpPost]
        [Route("api/user/change/password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            var IsKartUser = await UserService.CheckKartUserAsync(changeUserPasswordDto.UserId);
            if (!IsKartUser)
            {
                return Forbid();
            }
            await UserService.ChangeUserPasswordAsync(changeUserPasswordDto.UserId, changeUserPasswordDto.Password);
            return Ok();
        }

        #endregion

        #region Roles

        [HttpGet]
        [Route("api/users/roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await UserService.GetUserRoles();
            var dto = roles.Select(r => new UserRoleDto(r, false)).ToList();
            return Ok(dto);
        }


        [HttpGet]
        [Route("api/role/{roleId}/permissions/{filter}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetPermissions(int roleId, string filter, int pageIndex, int pageSize)
        {
            var permissions = await UserService.GetPermissions(roleId, filter, pageIndex, pageSize);
            var dto = permissions.Select(p => new UserPermissionDto(p)).ToList();
            return Ok(dto);
        }

        [HttpGet]
        [Route("api/role/{roleId}/permissions/count/{filter}")]
        public async Task<IActionResult> GetPermissionsCount(int roleId, string filter)
        {
            var count = await UserService.GetPermissionsCount(roleId, filter);
            return Ok(count);
        }

        [HttpGet]
        [Route("api/role/{roleId}/permissions/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetPermissions(int roleId, int pageIndex, int pageSize)
        {
            var permissions = await UserService.GetPermissions(roleId, "$0", pageIndex, pageSize);
            var dto = permissions.Select(p => new UserPermissionDto(p)).ToList();
            return Ok(dto);
        }

        [HttpGet]
        [Route("api/role/{roleId}/permissions/count")]
        public async Task<IActionResult> GetPermissionsCount(int roleId)
        {
            var count = await UserService.GetPermissionsCount(roleId, "$0");
            return Ok(count);
        }

        [HttpGet]
        [Route("api/role/available/{roleId}/{roleName}")]
        public async Task<IActionResult> IsRoleNameAvailable(int roleId, string roleName)
        {
            var available = await UserService.IsRoleNameAvailable(roleName, roleId);
            return Ok(available);
        }

        [HttpPost]
        [Route("api/role")]
        public async Task<IActionResult> AddRole([FromBody] UserRole role)
        {
            await UserService.AddRole(role);
            return Ok();
        }

        [HttpPut]
        [Route("api/role")]
        public async Task<IActionResult> UpdatePermissions([FromBody] UserRole role)
        {
            await UserService.UpdateRole(role);
            return Ok();
        }

        [HttpGet]
        [Route("api/check/kart/user/{userId}")]
        public async Task<IActionResult> CheckKartUser(int userId)
        {
            bool IsKartUser = await UserService.CheckKartUserAsync(userId);
            return Ok(IsKartUser);
        }


        #endregion

        #region Permissions

        [HttpGet]
        [Route("api/permissions/count/{filter}")]
        public async Task<IActionResult> GetPermissionsCount(string filter)
        {
            var count = await UserService.GetPermissionsCount(filter);
            return Ok(count);
        }

        [HttpGet]
        [Route("api/permissions/{filter}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetPermissions(string filter, int pageIndex, int pageSize)
        {
            var permissions = await UserService.GetPermissions(filter, pageIndex, pageSize);
            var dto = permissions.Select(p => new UserPermissionDto(p)).ToList();
            return Ok(dto);
        }

        [HttpGet]
        [Route("api/permissions/count")]
        public async Task<IActionResult> GetPermissionsCount()
        {
            var count = await UserService.GetPermissionsCount("$0");
            return Ok(count);
        }

        [HttpGet]
        [Route("api/permissions/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetPermissions(int pageIndex, int pageSize)
        {
            var permissions = await UserService.GetPermissions("$0", pageIndex, pageSize);
            var dto = permissions.Select(p => new UserPermissionDto(p)).ToList();
            return Ok(dto);
        }

        [HttpPost]
        [Route("api/permission/add")]
        public async Task<IActionResult> AddPermissions([FromBody] UserPermissionDto userPermissionDto)
        {
            await UserService.AddPermission(userPermissionDto.GetPermission());
            return Ok();
        }

        [HttpGet]
        [Route("api/permissions/{permissionId}/dependencies")]
        public async Task<IActionResult> GetPermissionDependencies(int permissionId)
        {
            var permissions = await UserService.GetPermissionDependencies(permissionId);
            var dto = permissions.Select(p => new UserPermissionDto(p)).ToList();
            return Ok(dto);
        }


        #endregion
    }
}