using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.User;

namespace Btf.Web.Api.DTO
{
    public class UserPermissionDto
    {
        public UserPermissionDto()
        {

        }
        public UserPermissionDto(Permission permission)
        {
            AccessUrl = permission.AccessUrl;
            Id = permission.Id;
            PermissionCode = permission.PermissionCode;
            PermissionName = permission.PermissionName;
            PermissionType = permission.PermissionType;
        }

        public UserPermissionDto(UserRolePermission userRole)
        {
            var permission = userRole.Permission;
            UserRolePermissionId = userRole.Id;
            AccessUrl = permission.AccessUrl;
            Id = permission.Id;
            PermissionCode = permission.PermissionCode;
            PermissionName = permission.PermissionName;
            PermissionType = permission.PermissionType;
        }

        public Permission GetPermission()
        {
            var permission = new Permission();
            permission.AccessUrl = AccessUrl;
            permission.PermissionCode = PermissionCode;
            permission.PermissionName = PermissionName;
            permission.PermissionType = PermissionType;
            return permission;
        }

        public int UserRolePermissionId { get; set; }
        public string AccessUrl { get; set; }
        public int Id { get;  set; }
        public string PermissionCode { get;  set; }
        public string PermissionName { get;  set; }
        public string PermissionType { get;  set; }
    }
}
