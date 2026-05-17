// Services/IBlobService.cs
namespace Portafolio.Services.Storage;
public interface IBlobService
{
    Task<string> UploadFileAsync(
        IFormFile file,
        string containerName
    );
    Task<string> GenerateDownloadUrlAsync(
        string blobName,
        string containerName
    );
}