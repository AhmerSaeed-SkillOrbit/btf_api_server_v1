using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class UserRolePermission : BaseEntity
    {
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
