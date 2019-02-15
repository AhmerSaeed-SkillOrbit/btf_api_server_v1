// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using Btf.Utilities.Configuration;

namespace Btf.IdentityServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("API", "Platform")
                {
                    ApiSecrets = { new Secret("4566e662-6670-406f-aea0-bd3b1c2d257b".Sha256()) }

                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(IConfigurationSection configuration)
        {
            // client credentials client
            return new List<Client>
            {
                // resource owner password grant clients
                new Client
                {
                    ClientId = "ro.web.client",
                    AccessTokenLifetime = configuration.GetValue<int>("TokenExpiry"), //set token expiry to 16 hours
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins = configuration.GetSection("Cors").Get<List<string>>(),
                    AccessTokenType = AccessTokenType.Reference,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 15770000, //6 months
                    SlidingRefreshTokenLifetime =  86400,//24 hours
                    ClientSecrets =
                    {
                        new Secret("6c018528-1f65-4603-977b-d87995ee7daf".Sha256())
                    },
                    AllowedScopes = { "API" }                   
                }

            };
        }

        //public static List<TestUser> GetUsers()
        //{
        //    return new List<TestUser>
        //    {
        //        new TestUser
        //        {
        //            SubjectId = "1",
        //            Username = "alice",
        //            Password = "password",

        //            Claims = new List<Claim>
        //            {
        //                new Claim("name", "Alice"),
        //                new Claim("website", "https://alice.com")
        //            }
        //        },
        //        new TestUser
        //        {
        //            SubjectId = "2",
        //            Username = "bob",
        //            Password = "password",

        //            Claims = new List<Claim>
        //            {
        //                new Claim("name", "Bob"),
        //                new Claim("website", "https://bob.com")
        //            }
        //        }
        //    };
        //}
    }
}