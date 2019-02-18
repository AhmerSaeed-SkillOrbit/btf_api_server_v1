using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.Log;

namespace Btf.Web.Api.DTO
{
    public class LogRequestDto
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string RequestUrl { get; set; }
        public DateTime RequestStartTime { get; set; }
        public DateTime RequestEndTime { get; set; }
        public int ResponseCode { get; set; }

        public LogRequestDto()
        {

        }

        public LogRequestDto(LogRequest logRequest)
        {
            Id = logRequest.Id;
            CreatedBy = logRequest.CreatedBy;
            UpdatedBy = logRequest.UpdatedBy;
            CreatedOn = logRequest.CreatedOn;
            UpdatedOn = logRequest.UpdatedOn;
            RequestUrl = logRequest.RequestUrl;
            RequestStartTime = logRequest.RequestStartTime;
            RequestEndTime = logRequest.RequestEndTime;
            ResponseCode = logRequest.ResponseCode;
        }
    }
}
