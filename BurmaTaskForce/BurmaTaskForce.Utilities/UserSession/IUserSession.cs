using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.User;

namespace Btf.Utilities.UserSession
{
    public interface IUserSession
    {
        User LoginUser { get; }
        List<Permission> Permissions { get; }
        void SetLoginUser(User loginUser);
        void SetPermissions(List<Permission> permissions);
    }
}
