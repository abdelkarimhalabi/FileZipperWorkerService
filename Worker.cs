using FileZipperWorkerService.Configurations;
using FileZipperWorkerService.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FileZipperWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerConfigurations _workerConfigurations;
        private readonly IFileService _fileService;

        public Worker(ILogger<Worker> logger , IOptions<WorkerConfigurations> workerConfigurations , IFileService fileService)
        {
            _logger = logger;
            _workerConfigurations = workerConfigurations.Value;
            _fileService = fileService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            while (!stoppingToken.IsCancellationRequested)
            {
                await _fileService.ZipAndSendFilesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(_workerConfigurations.DelayTimeInHours) , stoppingToken);
            }
        }
    }
}
