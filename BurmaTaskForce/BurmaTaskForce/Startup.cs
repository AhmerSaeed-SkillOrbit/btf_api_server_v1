using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Base;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Repositories;
using Btf.Services.EmailService;
using Btf.Services.LocationService;
using Btf.Services.LogService;
using Btf.Services.UserService;
using Btf.Utilities.Hash;
using Btf.Utilities.SixDigitKey;
using Btf.Utilities.UserSession;
using Swashbuckle.AspNetCore.Swagger;
using Btf.Web.Api.Authorization;
using Btf.Web.Api.LogRequestMiddleware;

namespace Btf.Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
            //dbContext
            services.AddDbContext<LogContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
            services.AddDbContext<UserContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
            services.AddDbContext<LocationContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DbConnection")));


            //add cors
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("AllowAll", policy =>
                {
                    // policy.WithOrigins("http://localhost:5003")
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddAuthentication("Bearer").AddIdentityServerAuthentication(op =>
           {
               op.Authority = Configuration.GetSection("Urls").GetValue<string>("Authority");
               op.RequireHttpsMetadata = false;
               op.ApiName = "API";
               op.SupportedTokens = SupportedTokens.Both;
               op.ApiSecret = "4566e662-6670-406f-aea0-bd3b1c2d257b";
               op.SaveToken = true;
               //op.TokenRetriever = (context) =>
               //{
               //    if (context.Headers.ContainsKey("Authorization"))
               //    {
               //        context.Headers.TryGetValue("Authorization", out var token);
               //        return token.ToString().Split()[1];
               //    }
               //    else if (context.Path.Value.StartsWith("/signalr") &&
               //            context.Query.TryGetValue("token", out StringValues token))
               //    {
               //        return token;
               //    }
               //    return "";
               //};
           });

            //For Custom Filters
           // services.AddMvc(options => options.Filters.Add(typeof(AuthorizationFilter), 1));

            //Without Custom Filters
            services.AddMvc();
            services.AddHttpContextAccessor();

            //if (Configuration.GetValue<string>("Permissions") == "on")
            //{
            //    //For Custom Filters
            //    services.AddMvc(options => options.Filters.Add(typeof(AuthorizationFilter), 0));
            //}
            //else
            //{
            //    //Without Custom Filters
            //    services.AddMvc();
            //}

            //swagger
            if (Configuration.GetValue<string>("Swagger") == "on")
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "VeeMed API", Version = "v1" });
                });

            }


            //VeeMed utitility services
            services.AddScoped<IPasswordHash, PasswordHash>();
            services.AddScoped<ISixDigitKeyProvider, SixDigitKeyProvider>();

            //VeeMed services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IUserSession, UserSession>();
            //VeeMed UOW
            services.AddScoped<IUnitOfWork<UserContext>, UnitOfWork<UserContext>>();
            services.AddScoped<IUnitOfWork<LocationContext>, UnitOfWork<LocationContext>>();
            services.AddScoped<IUnitOfWork<LogContext>, UnitOfWork<LogContext>>();

            //VeeMed repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            if (Configuration.GetValue<string>("Swagger") == "on")
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });

            }

            app.UseCors("AllowAll");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Adding Middleware
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();



            app.UseLogRequestMiddleware();
            app.UseAuthentication();
            app.UseAuthorizationMiddleware();
            app.UseMvc();
        }
    }
}
