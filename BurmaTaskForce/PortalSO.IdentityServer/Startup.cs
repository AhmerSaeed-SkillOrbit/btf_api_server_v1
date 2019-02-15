using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Base;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Repositories;
using Btf.IdentityServer.Extensions;
using Btf.IdentityServer.ResourceOwnerValidator;
using Btf.Services.EmailService;
using Btf.Services.UserService;
using Btf.Utilities.Hash;
using Btf.Utilities.SixDigitKey;
using Btf.Utilities.UserSession;

namespace Btf.IdentityServer
{
    public class Startup
    {
        private IConfiguration _configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = _configuration.GetConnectionString("AuthDb");
            var userDbConnecton = _configuration.GetConnectionString("DbConnection");

            //Injecting Dependencies chain to DI container
            services.AddDbContext<UserContext>(o => o.UseSqlServer(userDbConnecton));

            services.AddSingleton<IPasswordHash, PasswordHash>();
            services.AddSingleton<ISixDigitKeyProvider, SixDigitKeyProvider>();

            services.AddScoped<IUnitOfWork<UserContext>, UnitOfWork<UserContext>>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IUserSession, UserSession>();

            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // configure identity server with in-memory users, but EF stores for clients and scopes
            services.AddIdentityServer()

                //.AddTemporarySigningCredential()
                .AddCertificateFromStore(_configuration.GetSection("SigninKeyCredentials"))
                .AddInMemoryClients(Config.GetClients(_configuration.GetSection("ClientConfig")))
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())

                //.AddInMemoryCaching()
                //.AddClientStoreCache<ClientStore>()
                //.AddResourceStoreCache<ResourceStore>()
                // .AddCorsPolicyService<AllowAllCorsPolicy>()

                .AddResourceOwnerValidator<UserLoginValidator>()
              

                //.AddConfigurationStore(builder =>
                //    builder.UseSqlServer(connectionString, options =>
                //        options.MigrationsAssembly(migrationsAssembly)))
                ////.AddOperationalStore(builder =>
                //    builder.UseSqlServer(connectionString, options =>
                //        options.MigrationsAssembly(migrationsAssembly)));

                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    // this enables automatic token cleanup. this is optional.
                    //options.EnableTokenCleanup = true;
                    //options.TokenCleanupInterval = 30;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
            //Using Identity Server
            app.UseIdentityServer();
        }
    }
}
