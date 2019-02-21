using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Base;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Model.Log;

namespace Btf.Data.Repositories
{
    public class LogRepository : BaseRepository<LogRequest, LogContext>, ILogRepository
    {
        private DbSet<LogRequest> LogRequests;
        private DbSet<LogMsg> LogMsgs;

        public LogRepository(IUnitOfWork<LogContext> uow) : base(uow)
        {
            var context = (LogContext)uow.Context;
            LogRequests = context.LogRequests;
            LogMsgs = context.LogMsgs;
        }

        public async Task<List<LogMsg>> GetLogMessagesAsync(int logRequestId)
        {
            return await LogMsgs.Where((lm) => lm.IsActive && lm.LogRequestId == logRequestId).ToListAsync();
        }

        public async Task<int> GetLogMessagesCount(int logRequestId)
        {
            return await LogMsgs.CountAsync((lm) => lm.IsActive && lm.LogRequestId == logRequestId);
        }

        public async Task<List<LogRequest>> GetLogRequest(int pageIndex, int pageSize)
        {
           
                return await LogRequests
                    .Where((lr) => lr.IsActive)
                    .OrderByDescending(lr => lr.UpdatedOn)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            
           
        }

        public async Task<List<LogRequest>> GetLogRequestAsync(int pageIndex, int pageSize, string filter)
        {
           
                int responseCode;
                if (int.TryParse(filter, out responseCode))
                {
                    return await LogRequests
                    .Where((lr) => lr.IsActive && (lr.ResponseCode == responseCode || lr.RequestUrl.Contains(filter)) )
                    .OrderByDescending(lr => lr.UpdatedOn)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                }
                else
                {
                    return await LogRequests
                    .Where((lr) => lr.IsActive && lr.RequestUrl.Contains(filter))
                    .OrderByDescending(lr => lr.UpdatedOn)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                }
                
            
        
        }

        public async Task<int> GetLogRequestCount()
        {
            return await LogRequests.CountAsync((lr) => lr.IsActive);
        }

        public async Task<int> GetLogRequestCount(string filter)
        {
            int responseCode;
            if(int.TryParse(filter,out responseCode))
            {
                return await LogRequests.CountAsync((lr) => lr.IsActive && lr.ResponseCode == responseCode || lr.RequestUrl.Contains(filter));
            }
            else
            {
                return await LogRequests.CountAsync((lr) => lr.IsActive && lr.RequestUrl.Contains(filter)); 
            }
            
        }
    }
}
