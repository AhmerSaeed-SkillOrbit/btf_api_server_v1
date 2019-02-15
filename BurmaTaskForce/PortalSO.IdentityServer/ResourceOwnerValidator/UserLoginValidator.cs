using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.User;
using Btf.Services.UserService;

namespace Btf.IdentityServer.ResourceOwnerValidator
{
    public class UserLoginValidator : IResourceOwnerPasswordValidator
    {
        private IUserService userService;

        public UserLoginValidator(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            User user = await userService.CheckLogin(context.UserName, context.Password);
            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid credential");
            }
            else
            {
                context.Result = new GrantValidationResult(subject: user.UserGUID, authenticationMethod: "custom");
            }            
        }
    }
}
