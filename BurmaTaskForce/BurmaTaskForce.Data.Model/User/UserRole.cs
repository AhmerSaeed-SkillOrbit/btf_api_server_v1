using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class UserRole : BaseEntity
    {
        public UserRole()
        {
            Permissions = new List<UserRolePermission>();
        }
        public string Role { get; set; }
        public List<UserRolePermission> Permissions { get; set; }
    }
}
