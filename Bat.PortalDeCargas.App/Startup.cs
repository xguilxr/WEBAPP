using System.Collections.Generic;
using System.Globalization;
using Bat.PortalDeCargas.App.Configuration;
using Bat.PortalDeCargas.Domain.Configuration;
using Bat.PortalDeCargas.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
#if DEBUG
                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start"); //use it to launch ClientApp along with the API
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
#endif
            });

            app.UseRequestLocalization();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
            services.AddCors();
            services.AddControllersWithViews();
            services.AddTokenAuthentication(Configuration);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist/fuse"; });
            services.AddDbContext<PortalDeCargasContext>(config =>
                config.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.RegisterMapping(appConfig);
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                var cultures = new[]
                {
                    new CultureInfo("en-US"), new CultureInfo("pt-BR")
                };

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).WriteTo.MSSqlServer(
                Configuration.GetConnectionString("DefaultConnection"), new MSSqlServerSinkOptions
                {
                    SchemaName = Configuration.GetValue<string>("Serilog:SchemaName"),
                    TableName = Configuration.GetValue<string>("Serilog:TableName"),
                    AutoCreateSqlTable = true
                }, columnOptions: new ColumnOptions
                {
                    Id =
                    {
                        NonClusteredIndex = false
                    },
                    Store = new List<StandardColumn>(new[]
                    {
                        StandardColumn.Id, StandardColumn.Level, StandardColumn.Message, StandardColumn.Exception,
                        StandardColumn.Properties
                    }),
                    Properties =
                    {
                        ExcludeAdditionalProperties = true,
                        OmitElementIfEmpty = true,
                        UsePropertyKeyAsElementName = true
                    },
                    TimeStamp =
                    {
                        ConvertToUtc = false
                    }
                }).CreateLogger();
        }
    }
}
