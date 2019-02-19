using System;
using System.Collections.Generic;
using System.Text;
using Btf.Data.Model.Base;

namespace Btf.Data.Model.Log
{
    public class LogRequest : BaseEntity
    {
        public LogRequest()
        {
            LogMsgs = new List<LogMsg>();
        }

        public string RequestUrl { get; set; }
        public DateTime RequestStartTime { get; set; }
        public DateTime RequestEndTime { get; set; }
        public int ResponseCode { get; set; }
        public List<LogMsg> LogMsgs { get; set; }
    }
}
