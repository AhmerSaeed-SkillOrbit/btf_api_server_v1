using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Btf.Utilities.UserSession;

namespace Btf.Web.Api.Authorization
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IUserSession _userSession;

        public AuthorizationFilter(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //check for authrorization attribute
            var controllerInfo = context.ActionDescriptor as ControllerActionDescriptor;
            var hasAuthorizeAttr = controllerInfo.ControllerTypeInfo.CustomAttributes
                                        .Any(a => a.AttributeType == typeof(AuthorizeAttribute));

            //simply returns if authorization is not required
            if (!hasAuthorizeAttr) return;

            //check if details of login user are available
            if (_userSession.LoginUser == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            bool isAuthorized = false;

            //get user access
            //var access = _userSession.LoginUser.UserAccess;
            var permissions = _userSession.Permissions;

            if(permissions == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            //filter the permissions as we only have to check API permissions
            var apiPermissions = permissions.Where(p => p.PermissionType == PermissionTypes.API.ToString());

            foreach (var permission in apiPermissions)
            {
                //split http verb and api url
                var accessUrl = permission.AccessUrl.Split(";".ToCharArray());
                var httpVerb = accessUrl[0];

                if (accessUrl.Length < 1 ||
                    context.HttpContext.Request.Method.ToLower() != httpVerb.ToLower())
                    continue;

                var url = accessUrl[1];

                //check if the api url deos require parameters
                var splitUrls = url.Split("$".ToCharArray());
                var parametersCount = 0;
                if (splitUrls.Length > 1)
                {
                    url = splitUrls[0].Substring(0, splitUrls[0].Length - 1);
                    parametersCount = splitUrls.Length - 1;
                }
                //build the regular expression
                string regStr = @"^(" + url.Replace(@"/", @"\/") + ")";
                for (int i = 1; i <= parametersCount; ++i)
                {
                    //regStr += @"(\/(.+))";
                    regStr += @"(\/(((?!\/).)+))";
                    var splitUrl = splitUrls[i];
                    if (splitUrl.Length > 1)
                    {
                        if (splitUrl.Substring(splitUrl.Length - 1, 1) == "/")
                        {
                            splitUrl = splitUrl.Substring(0, splitUrl.Length - 1);
                        }
                        regStr += @"(" + splitUrl.Replace(@"/", @"\/") + ")";
                    }

                }

                //end of regular expression
                regStr += @"\/$";

                //check if login user have access to this URL
                Regex reg = new Regex(regStr);
                isAuthorized = reg.Match(context.HttpContext.Request.Path + "/").Success;
                if (isAuthorized)
                    break; //the user has access
            }

            //check if authroized to access the api 
            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
