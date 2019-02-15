using Btf.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Project
{
    public class ProjectTeam : BaseEntity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string ProjectRole { get; set; }

        public Project Project { get; set; }
        public User.User User { get; set; }
    }
}
