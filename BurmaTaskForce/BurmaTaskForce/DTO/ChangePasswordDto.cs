using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Btf.Web.Api.DTO
{
    public class ChangePasswordDto
    {
        public string VerificationKey { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
