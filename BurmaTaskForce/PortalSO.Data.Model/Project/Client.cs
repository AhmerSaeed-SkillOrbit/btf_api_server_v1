using Btf.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Project
{
    public class Client : BaseEntity
    {
        public Client()
        {
            Projects = new List<Project>();
        }
        public string Name { get; set; }

        public List<Project> Projects { get; set; }
    }
}
