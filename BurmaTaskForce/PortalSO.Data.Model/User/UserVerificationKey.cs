using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.User
{
    public class UserVerificationKey : BaseEntity 
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string VerificationKey { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsUsed { get; set; }
    }
}
