using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Config;
using HwInf.BusinessLogic.Interfaces;
using HwInf.DataAccess;
using HwInf.DataAccess.Context;
using HwInf.DataAccess.Interfaces;
using HwInf.Services;
using HwInf.Services.Config;
using HwInf.Services.MailService;
using HwInf.Services.PdfService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace HwInf.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Configuration.GetSection("App:JWT").Get<JwtConfig>());
            services.AddSingleton(Configuration.GetSection("Services:Mail").Get<MailConfig>());
            services.AddTransient<IPrincipal>(
                provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddEntityFrameworkNpgsql().AddDbContext<HwInfContext>((options => options.UseNpgsql(Configuration["App:Connection:Value"])));
            services.AddScoped<IDataAccessLayer, DataAccessLayer>();
            services.AddScoped<IBusinessLogic, BusinessLogic.BusinessLogic>();
            services.AddScoped<IAccessoryBusinessLogic, AccessoryBusinessLogic>();
            services.AddScoped<ICustomFieldsBusinessLogic, CustomFieldsBusinessLogic>();
            services.AddScoped<IDamageBusinessLogic, DamageBusinessLogic>();
            services.AddScoped<IDeviceBusinessLogic, DeviceBusinessLogic>();
            services.AddScoped<IOrderBusinessLogic, OrderBusinessLogic>();
            services.AddScoped<ISettingBusinessLogic, SettingBusinessLogic>();
            services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
            services.AddScoped<IBusinessLogicFacade, BusinessLogicFacade>();
            services.AddScoped<IBusinessLogicPrincipal, BusinessLogicPrincipal>();

            services.AddScoped<IPdfService, PDFCreator>();
            services.AddScoped<IMailService, Mail>();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var security = new Dictionary<string, IEnumerable<string>>
            {
                {"Bearer", new string[] { }},
            };
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HwInf", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtConfig.Current.Issuer,
                        ValidAudience = JwtConfig.Current.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(JwtConfig.Current.SecretKey))
                    };
                    
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            loggerFactory.AddLog4Net();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HwInf");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.Options.StartupTimeout = new TimeSpan(0, 0, 360);
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
