using FileZipperWorkerService.Services.Interfaces;
using FileZipperWorkerService.Services;
using FileZipperWorkerService.Configurations;

namespace FileZipperWorkerService.DI
{
    internal static class IServiceCollectionExtension
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }

        internal static IServiceCollection AddConfigurations(this IServiceCollection services , IConfiguration configuration)
        {
            services.Configure<EmailConfigurations>(configuration.GetSection(nameof(EmailConfigurations)));
            services.Configure<WorkerConfigurations>(configuration.GetSection(nameof(WorkerConfigurations)));

            return services;
        }
    }
}
