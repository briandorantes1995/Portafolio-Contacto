using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace Portafolio.Services.Storage;

public class BlobService : IBlobService
{
    private readonly IConfiguration _configuration;

    public BlobService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName)
    {
        var connectionString = _configuration["AzureBlob:ConnectionString"];

        var blobContainerClient = new BlobContainerClient(connectionString, containerName);

        await blobContainerClient.CreateIfNotExistsAsync();

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var blobClient = blobContainerClient.GetBlobClient(fileName);

        using var stream = file.OpenReadStream();

        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    
    public async Task<string> GenerateDownloadUrlAsync(string blobName, string containerName)
    {
        var connectionString = _configuration["AzureBlob:ConnectionString"];

        var blobContainerClient = new BlobContainerClient(connectionString, containerName);

        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }
}