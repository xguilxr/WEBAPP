using Bat.PortalDeCargas.Domain.Configuration;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.Services.Azure;
using Bat.PortalDeCargas.Domain.Services.Dimensions;
using Bat.PortalDeCargas.Domain.Services.Files;
using Bat.PortalDeCargas.Domain.Services.Template;
using Bat.PortalDeCargas.Domain.Services.UserService;
using Bat.PortalDeCargas.Domain.Services.UserServices;
using Bat.PortalDeCargas.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Bat.PortalDeCargas.App.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterMapping(this IServiceCollection services, IAppConfiguration appConfig)
        {
            services.AddSingleton(appConfig).AddSingleton<IFileServiceConstructor, FileServiceConstructor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>().AddScoped<IDimensionService, DimensionService>()
                .AddScoped<IAzureIntegrationService, AzureIntegrationService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<ITemplateService, TemplateService>().AddScoped<IUsersService, UsersService>()
                .AddScoped<IPasswordService, PasswordService>()
                .AddScoped<IDimensionDomainUploadService, DimensionDomainUploadService>()
                .AddScoped<IDimensionFileValidateService, DimensionFileValidateService>();
        }
    }
}
