using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.User;

namespace Btf.Utilities.UserSession
{
    public class UserSession : IUserSession
    {
        private User _logInUser = null;
        public User LoginUser => _logInUser;

        private List<Permission> _permissions;
        public List<Permission> Permissions => _permissions;

        public void SetLoginUser(User loginUser)
        {
            if (_logInUser != null)
                throw new Exception("User session already exist.");

            _logInUser = loginUser;
        }

        public void SetPermissions(List<Permission> permissions)
        {
            _permissions = permissions;
        }
    }
}
