using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.Log;

namespace Btf.Web.Api.DTO
{
    public class LogMsgDto
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int LogRequestId { get; set; }
        public string LogMsgType { get; set; }
        public string Msg { get; set; }
        public string StackTrace { get; set; }

        public LogMsgDto()
        {

        }

        public LogMsgDto(LogMsg logMsg)
        {
            Id = logMsg.Id;
            CreatedBy = logMsg.CreatedBy;
            UpdatedBy = logMsg.UpdatedBy;
            CreatedOn = logMsg.CreatedOn;
            UpdatedOn = logMsg.UpdatedOn;
            LogRequestId = logMsg.LogRequestId;
            LogMsgType = logMsg.LogMsgType;
            Msg = logMsg.Msg;
            StackTrace = logMsg.StackTrace;
        }
    }
}
