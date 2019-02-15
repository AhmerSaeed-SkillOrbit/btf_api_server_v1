using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class Country : BaseEntity
    {
        public Country()
        {
            States = new List<State>();
        }
        public string CountryName { get; set; }

        public List<State> States { get; set; }
    }
}
