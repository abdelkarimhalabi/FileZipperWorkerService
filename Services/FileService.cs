using System.IO.Compression;
using FileZipperWorkerService.Configurations;
using FileZipperWorkerService.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FileZipperWorkerService.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly WorkerConfigurations _workerConfigurations;
        private readonly IEmailService _emailService;

        public FileService(ILogger<FileService> logger , IOptions<WorkerConfigurations> workerConfigurations , IEmailService emailService)
        {
            _logger = logger;
            _workerConfigurations = workerConfigurations.Value;
            _emailService = emailService;
        }

        public async Task ZipAndSendFilesAsync(CancellationToken cancellationToken)
        {
            try
            {
                bool targetFolderPathCheck = IsTargetFolderPathValid();

                if (targetFolderPathCheck)
                {
                    string zipFilePath = CreateZipFile();
                    await _emailService.SendEmailWithAttachmentAsync(zipFilePath , cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Zipping and sending the email.");
                throw;
            }
        }

        #region Private
        private bool IsTargetFolderPathValid()
        {
            if (!Directory.Exists(_workerConfigurations.TargetFolderPath))
            {
                _logger.LogInformation($"Folder with Path {_workerConfigurations.TargetFolderPath} doesn't exists.");
                return false;
            }

            return true;
        }

        private string CreateZipFile()
        {
            string targetFolderPath = _workerConfigurations.TargetFolderPath;
            string zipFilePath = Path.Combine(Path.GetTempPath(), "FilesArchive.zip");

            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }

            _logger.LogInformation("Creating zip file at {path}", zipFilePath);

            ZipFile.CreateFromDirectory(_workerConfigurations.TargetFolderPath, zipFilePath);
            int zippedFilesCount = Directory.EnumerateFiles(targetFolderPath, "*", SearchOption.AllDirectories).Count();
            _logger.LogInformation($"Zip file created successfully, Number of zipped files : {zippedFilesCount}.");

            return zipFilePath;
        }

        #endregion
    }
}
