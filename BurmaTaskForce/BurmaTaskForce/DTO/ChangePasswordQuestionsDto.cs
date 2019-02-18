using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Btf.Web.Api.DTO
{
    public class ChangePasswordQuestionsDto
    {
        public string VerificationKey { get; set; }
        public string NewPassword { get; set; }
        public string SecretQuestion1 { get; set; }
        public string SecretAnswer1 { get; set; }
        public string SecretQuestion2 { get; set; }
        public string SecretAnswer2 { get; set; }
    }
}
