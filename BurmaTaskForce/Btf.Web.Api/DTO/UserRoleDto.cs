using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.User;

namespace Btf.Web.Api.DTO
{
    public class UserRoleDto
    {
        public string Role { get; set; }
        public int UserAccessId { get; set; }
        public int UserRoleId { get; set; }
        public List<UserPermissionDto> Permissions { get; set; }

        public UserRoleDto(UserRole role, bool permissionRequired = true)
        {
            Permissions = new List<UserPermissionDto>();
            UserRoleId = role.Id;
            Role = role.Role;
            if (permissionRequired && role.Permissions != null)
            {
                Permissions = role.Permissions.Where(p => p.IsActive).Select(p => new UserPermissionDto(p.Permission)).ToList();
            }
        }

        public UserRoleDto(UserRole role, string permissionType)
        {
            Permissions = new List<UserPermissionDto>();
            UserRoleId = role.Id;
            Role = role.Role;
            if (role.Permissions != null)
            {
                Permissions = role.Permissions.Where(p => p.Permission.PermissionType == permissionType && p.IsActive)
                                              .Select(p => new UserPermissionDto(p.Permission)).ToList();
            }
        }

        public UserRoleDto(UserRole role, int userAccessId, bool permissionRequired = true)
        {
            Permissions = new List<UserPermissionDto>();
            UserAccessId = userAccessId;
            UserRoleId = role.Id;
            Role = role.Role;
            if (permissionRequired && role.Permissions != null)
            {
                Permissions = role.Permissions.Where(p => p.IsActive).Select(p => new UserPermissionDto(p.Permission)).ToList();
            }
        }
    }
}
