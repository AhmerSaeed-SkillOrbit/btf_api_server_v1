using Btf.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Project
{
    public class Project : BaseEntity
    {
        public Project()
        {
            ProjectTasks = new List<ProjectTask>();
            ProjectTeams = new List<ProjectTeam>();
        }

        public string Name { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public Client Client { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
        public List<ProjectTeam> ProjectTeams { get; set; }
    }
}
