using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class State : BaseEntity
    {
        public State()
        {
            Cities = new List<City>();
        }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public string StateName { get; set; }

        public List<City> Cities { get; set; }
        public string TimeZoneCode { get; set; }
        public string TimeZoneDescription { get; set; }
        public int UtcOffsetInSeconds { get; set; }
        public int UtcDSTOffsetInSeconds { get; set; }
    }
}
