namespace FileZipperWorkerService.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailWithAttachmentAsync(string zipFilePath , CancellationToken cancellationToken);
    }
}
