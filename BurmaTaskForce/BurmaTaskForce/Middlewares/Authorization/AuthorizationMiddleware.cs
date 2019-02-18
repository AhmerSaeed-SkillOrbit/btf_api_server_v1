using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Btf.Data.Model.User;
using Btf.Services.UserService;
using Btf.Utilities.UserSession;
using Btf.Web.Api.DTO;

namespace Btf.Web.Api.Authorization
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IUserSession userSession)
        {
            //_userService = userService;
            //ToDo: implement caching 
            //get permission list
            var user = await LoadTokenInfo(context, userService);

            if (user != null)
            {

                // bool isAuthorized = false;
                var access = await userService.GetUserAccess(user.Id);
                var permissionIds = access.SelectMany(a => a.UserRole.Permissions.Select(p => p.PermissionId)).ToList();
                var permissionDependencies = await userService.GetPermissionDependenciesAsync(permissionIds);

                foreach (var a in access)
                {
                    permissionDependencies.AddRange(a.UserRole.Permissions.Select(p => p.Permission));
                }

                userSession.SetLoginUser(user);
                userSession.SetPermissions(permissionDependencies);
            }

            //call the next component in pipeline
            await _next(context);
        }

        protected async Task<Data.Model.User.User> LoadTokenInfo(HttpContext context, IUserService userService)
        {
            ClaimsPrincipal Caller = context.User as ClaimsPrincipal;
            if (Caller != null)
            {
                var claim = Caller.FindFirst("client_id");
                if (claim != null)
                {

                    var clientId = claim.Value;

                    if (!string.IsNullOrWhiteSpace(clientId))
                    {
                        var Client = new ClientDto
                        {
                            ClientId = clientId
                        };

                        //check if it's a user access token
                        var subjectClaim = Caller.FindFirst("sub");

                        if (subjectClaim != null)
                        {
                            //get the user info based on guid
                            return await userService.GetByUserGuid(subjectClaim.Value);
                        }
                    }
                }
            }

            return null;
        }
    }
}