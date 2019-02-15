using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class City : BaseEntity
    {
       
        public int StateId { get; set; }
        public State State { get; set; }
        public string CityName { get; set; }
    }
}
