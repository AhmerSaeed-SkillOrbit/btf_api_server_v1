using Btf.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Project
{
    public class ProjectTask : BaseEntity
    {
        public ProjectTask()
        {
            ProjectActivities = new List<ProjectActivity>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Project Project { get; set; }
        public List<ProjectActivity> ProjectActivities { get; set; }
    }
}
