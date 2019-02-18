using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Btf.Web.Api.DTO
{
    public class UserNameGUIDDto
    {
        public string userGUID { get; set; }
        public string Name { get; set; }

        public UserNameGUIDDto()
        {

        }

        public UserNameGUIDDto(KeyValuePair<string,string> user)
        {
            userGUID = user.Key;
            Name = user.Value;
        }
    }
}
