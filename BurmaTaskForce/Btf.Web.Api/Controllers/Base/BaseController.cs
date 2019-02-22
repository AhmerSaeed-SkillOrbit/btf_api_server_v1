using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Btf.Data.Model.User;
using Btf.Services.UserService;
using Btf.Web.Api.DTO;

namespace Btf.Web.Api.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        protected User LoginUser;
        protected IUserService UserService;

        public BaseController(IUserService userService)
        {
            this.UserService = userService;
            LoadTokenInfo();
        }

        protected void LoadTokenInfo()
        {
            LoginUser = UserService.LoginUser;
            //ClaimsPrincipal Caller = User as ClaimsPrincipal;
            //if (Caller != null)
            //{
            //    var claim = Caller.FindFirst("client_id");
            //    if (claim != null)
            //    {

            //        var clientId = claim.Value;

            //        if (!string.IsNullOrWhiteSpace(clientId))
            //        {
            //            Client = new ClientDto
            //            {
            //                ClientId = clientId
            //            };

            //            //check if it's a user access token
            //            var subjectClaim = Caller.FindFirst("sub");

            //            if (subjectClaim != null)
            //            {
            //                //get the user info based on guid
            //                LoginUser = UserService.GetByUserGuid(subjectClaim.Value).Result;
            //            }
            //        }
            //    }
            //}
        }
    }
}