using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Utilities.Configuration
{
    public class EmailConfig
    {
        public string EmailVerificationAddressAdmin { get; set; }
        public string EmailVerificationAddressWeb { get; set; }
        public string SenderAddress { get; set; }
        public string AccessKey { get; set; }
        public string AccessId { get; set; }
    }
}
