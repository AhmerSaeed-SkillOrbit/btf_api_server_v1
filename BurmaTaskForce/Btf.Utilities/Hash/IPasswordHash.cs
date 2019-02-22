using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Utilities.Hash
{
    public interface IPasswordHash
    {
        string GetHash(string password);
    }
}
