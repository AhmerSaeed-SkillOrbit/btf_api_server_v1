using System;
using System.Threading.Tasks;

namespace Btf.Services.EmailService
{
    public interface IEmailService
    {
        Task<Boolean> Send(string to, string subject, string[] data, string userType = "veedoc", bool isEmailVerification = true);
        Task SenduserRegistrationInfo(string to, string subject, string[] parameters);
        Task SendRadiologyRequest(string to, string subject, string[] parameters);
        Task SendSupervisorRadiologyRequest(string to, string subject, string[] parameters);
    }
}
