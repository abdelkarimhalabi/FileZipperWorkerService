namespace FileZipperWorkerService.Services.Interfaces
{
    public interface IFileService
    {
        Task ZipAndSendFilesAsync(CancellationToken cancellationToken);
    }
}
