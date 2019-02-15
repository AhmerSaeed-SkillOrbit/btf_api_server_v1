using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Btf.Data.Model.Base;
using Btf.Data.Model.Project;

namespace Btf.Data.Model.User
{
    /**
     * User Class 
     * */
    public class User : BaseEntity
    {
        public User()
        {
            UserAccess = new List<Model.User.UserAccess>();
            ProjectTeams = new List<ProjectTeam>();
            ProjectActivities = new List<ProjectActivity>();
        }
        public string UserGUID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }

        public string SecretQuestion1 { get; set; }
        public string SecretAnswer1 { get; set; }
        public string SecretQuestion2 { get; set; }
        public string SecretAnswer2 { get; set; }

        public string Address { get; set; }
        public string Address1 { get; set; }
        public string ZipCode { get; set; }

        public List<UserAccess> UserAccess { get; set; }
        public List<ProjectTeam> ProjectTeams { get; set; }
        public List<ProjectActivity> ProjectActivities { get; set; }
    }
}
