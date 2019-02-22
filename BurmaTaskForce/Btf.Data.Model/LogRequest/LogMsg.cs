using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.Log
{
    public class LogMsg : BaseEntity
    {
        public int LogRequestId { get; set; }
        public LogRequest LogRequest { get; set; }
        public string LogMsgType { get; set; }
        public string Msg { get; set; }
        public string StackTrace { get; set; }
    }
}
