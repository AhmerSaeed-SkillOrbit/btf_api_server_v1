using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.Log;

namespace Btf.Data.Contracts.Interfaces
{
    public interface ILogRepository : IRepository<LogRequest>
    {
        Task<int> GetLogRequestCount();
        Task<int> GetLogRequestCount(string filter);
        Task<List<LogRequest>> GetLogRequest(int pageIndex, int pageSize);
        Task<List<LogRequest>> GetLogRequestAsync(int pageIndex, int pageSize, string filter);
        Task<int> GetLogMessagesCount(int logRequestId);
        Task<List<LogMsg>> GetLogMessagesAsync(int logRequestId);
    }
}
