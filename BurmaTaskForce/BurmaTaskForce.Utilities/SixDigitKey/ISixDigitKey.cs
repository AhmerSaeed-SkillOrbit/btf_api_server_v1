using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Utilities.SixDigitKey
{
    public interface ISixDigitKeyProvider
    {
        string GenerateNewKey();
    }
}
