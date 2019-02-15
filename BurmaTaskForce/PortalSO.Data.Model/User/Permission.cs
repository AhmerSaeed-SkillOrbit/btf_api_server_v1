using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class Permission : BaseEntity
    {
        public string PermissionCode { get; set; }
        public string PermissionName { get; set; }
        public string PermissionType { get; set; }
        public string AccessUrl { get; set; }
    }
}
