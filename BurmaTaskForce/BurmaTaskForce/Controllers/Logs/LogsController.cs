using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.Log;
using Btf.Services.LogService;
using Btf.Services.UserService;
using Btf.Web.Api.Controllers.Base;
using Btf.Web.Api.DTO;

namespace Btf.Web.Api.Controllers.Logs
{
    
    [Authorize]
    public class LogsController : BaseController
    {
        private ILogService _logService;
        public LogsController(IUserService userService,ILogService logService) : base(userService)
        {
            _logService = logService;
        }

        [HttpGet]
        [Route("api/log/requests/count")]
        public async Task<IActionResult> GetLogRequestsCount()
        {
            int count = await _logService.GetLogRequestCount();
            return Ok(count);
        }

        [HttpGet]
        [Route("api/log/requests/filter/count")]
        public async Task<IActionResult> GetLogRequestsCount([FromQuery]string filter)
        {
            int count = await _logService.GetLogRequestCount(filter);
            return Ok(count);
        }

        [HttpGet]
        [Route("api/log/requests/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetLogRequests(int pageIndex,int pageSize)
        {
            List<LogRequest> list = await _logService.GetLogRequest(pageIndex, pageSize);
            List<LogRequestDto> dtoList = new List<LogRequestDto>();
            foreach (var item in list)
            {
                dtoList.Add(new LogRequestDto(item));
            }

            return Ok(dtoList);
        }

        [HttpGet]
        [Route("api/log/requests/filter/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetLogRequests(int pageIndex, int pageSize,[FromQuery]string filter)
        {
            List<LogRequest> list = await _logService.GetLogRequestAsync(pageIndex, pageSize,filter);
            List<LogRequestDto> dtoList = new List<LogRequestDto>();
            foreach (var item in list)
            {
                dtoList.Add(new LogRequestDto(item));
            }

            return Ok(dtoList);
        }

        [HttpGet]
        [Route("api/log/messages/count/{logRequestId}")]
        public async Task<IActionResult> GetLogMessagesCount(int logRequestId)
        {
            int count = await _logService.GetLogMessagesCount(logRequestId);
            return Ok(count);
        }

        [HttpGet]
        [Route("api/log/messages/{logRequestId}")]
        public async Task<IActionResult> GetLogMessages(int logRequestId)
        {
            List<LogMsg> list = await _logService.GetLogMessages(logRequestId);
            List<LogMsgDto> dtoList = new List<LogMsgDto>();
            foreach (var item in list)
            {
                dtoList.Add(new LogMsgDto(item));
            }

            return Ok(dtoList);
        }

    }
}
