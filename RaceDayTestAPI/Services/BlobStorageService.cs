
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDayTestAPI.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(
            BlobServiceClient blobServiceClient,
            IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;

            _containerName = configuration["AzureBlobStorage:ContainerName"]
                ?? "raceday-images";
        }

        public async Task<string> UploadImageAsync(
            Stream fileStream,
            string fileName,
            string contentType)
        {
            BlobContainerClient containerClient =
                _blobServiceClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync(
                PublicAccessType.None);

            string uniqueFileName =
                Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            BlobClient blobClient =
                containerClient.GetBlobClient(uniqueFileName);

            await blobClient.UploadAsync(
                fileStream,
                new BlobHttpHeaders
                {
                    ContentType = contentType
                });

            return blobClient.Uri.ToString();
        }
    }
}
