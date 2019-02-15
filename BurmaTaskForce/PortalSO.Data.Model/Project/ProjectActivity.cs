using Btf.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Project
{
    public class ProjectActivity : BaseEntity
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public int TimeInMinutes { get; set; }
        public int UserId { get; set; }

        public ProjectTask Task { get; set; }
        public User.User User { get; set; }
    }
}
