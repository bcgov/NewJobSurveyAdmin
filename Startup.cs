using System.Net.Http;
using System;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using NewJobSurveyAdmin.Services.CallWeb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sieve.Services;
using Sieve.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace NewJobSurveyAdmin
{
    public class Startup
    {
        public static readonly string HttpClientName = "HttpClient";

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Environment = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add
        // services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                        .WithMethods("GET", "PUT", "PATCH", "POST", "DELETE", "OPTIONS")
                      //   .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("X-Pagination"));
            });

            services.AddControllersWithViews();

            services.Configure<CallWebServiceOptions>(Configuration.GetSection("CallWebApi"));
            services.AddSingleton<CallWebService>();

            services.AddSingleton<CsvService>();

            services.Configure<EmailServiceOptions>(Configuration.GetSection("Email"));
            services.AddSingleton<EmailService>();

            services.Configure<EmployeeInfoLookupServiceOptions>(Configuration.GetSection("LdapLookup"));
            services.AddSingleton<EmployeeInfoLookupService>();

            services.AddScoped<EmployeeReconciliationService>();

            services.AddSingleton<LocalFileService>();

            services.AddScoped<LoggingService>();

            services.AddDbContext<NewJobSurveyAdminContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("NewJobSurveyAdmin"))
            );

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
            services.AddScoped<SieveProcessor>();

            // TODO: Is this still important?
            // services
            //     .AddAuthentication(options =>
            //         Authentication.SetAuthenticationOptions(options))
            //     .AddJwtBearer(options =>
            //     {
            //         options.Authority = Configuration["Authentication:Authority"];
            //         options.Audience = Configuration["Authentication:Audience"];
            //     }

            //     // Authentication.SetJwtBearerOptions(
            //     //     options,
            //     //     Configuration.GetValue<string>("Authentication:Authority")
            //     // )
            //     );

            services
                .AddAuthentication(options =>
                    Authentication.SetAuthenticationOptions(options))
                .AddJwtBearer(options =>
                    Authentication.SetJwtBearerOptions(
                        options,
                        Configuration.GetValue<string>("Authentication:Authority")
                    )
                );

            services
                .AddAuthorization(options =>
                    Authentication.SetAuthorizationOptions(
                        options,
                        Configuration.GetValue<string>("Authentication:RoleName")
                    )
                );


            // Add an HttpClient.
            services.AddHttpClient(HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler();
                    // Ignore certificate errors ON DEV ONLY.
                    if (Environment.IsDevelopment())
                    {
                        handler.ServerCertificateCustomValidationCallback +=
                        (httpRequestMessage, cert, cetChain, policyErrors) => true;
                    }
                    return handler;
                });

            // // In production, the React files will be served from this directory
            // services.AddSpaStaticFiles(configuration =>
            // {
            //     configuration.RootPath = "ClientApp/build";
            // });
        }

        // This method gets called by the runtime. Use this method to configure
        // the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change
                // this for production scenarios, see
                // https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            // app.UseStaticFiles();
            // app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            // app.UseSpa(spa =>
            // {
            //     spa.Options.SourcePath = "ClientApp";

            //     if (env.IsDevelopment())
            //     {
            //         spa.UseReactDevelopmentServer(npmScript: "start");
            //     }
            // });
        }
    }
}
