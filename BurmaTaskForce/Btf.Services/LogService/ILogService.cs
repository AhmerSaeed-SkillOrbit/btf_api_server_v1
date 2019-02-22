using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.Log;

namespace Btf.Services.LogService
{
    public interface ILogService 
    {
        void StartLog(string requestUrl);
        void EndLog(int responseCode);
        void Log(string logMsg, LogTypes logType);
        void LogError(string errorMsg, string stackTrace);
        Task<int> GetLogRequestCount();
        Task<int> GetLogRequestCount(string filter);
        Task<List<LogRequest>> GetLogRequest(int pageIndex, int pageSize);
        Task<List<LogRequest>> GetLogRequestAsync(int pageIndex, int pageSize, string filter);
        Task<int> GetLogMessagesCount(int logRequestId);
        Task<List<LogMsg>> GetLogMessages(int logRequestId);
    }
}
