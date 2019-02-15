using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Utilities.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {
        }
    }
}
