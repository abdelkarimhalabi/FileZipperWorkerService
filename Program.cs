using FileZipperWorkerService;
using FileZipperWorkerService.DI;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Starting up FileZipperWorkerService...");

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Logging.AddNLog();
    builder.Services.AddConfigurations(builder.Configuration);
    builder.Services.AddServices();
    builder.Services.AddHostedService<Worker>();
    

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Unhandled exception occurred in Program.cs");
throw;
}
finally
{
    LogManager.Shutdown();
}