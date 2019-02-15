using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class PermissionDependency : BaseEntity
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public int ChildPermissionId { get; set; }
        public Permission ChildPermission { get; set; }
    }
}
