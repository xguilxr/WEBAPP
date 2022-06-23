using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bat.PortalDeCargas.Domain.Configuration;
using Microsoft.Extensions.Logging;

namespace Bat.PortalDeCargas.Domain.Services.Azure
{
    public class AzureIntegrationService : IAzureIntegrationService
    {
        private readonly IAppConfiguration _config;
        private readonly ILogger<AzureIntegrationService> _logger;

        public AzureIntegrationService(IAppConfiguration config, ILogger<AzureIntegrationService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task PostBlobAsync(byte[] fileContent, Entities.Template template)
        {
            var blobServiceClient = new BlobServiceClient(_config.BlobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(template.TemplateBlobUrl.ToLower());
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
            var blobClient =
                blobContainerClient.GetBlobClient($"{template.TemplateName}{(string)template.TemplateFileFormat}");

            using var stream = new MemoryStream(fileContent);
            await blobClient.UploadAsync(new BinaryData(fileContent), true);
        }

        public async Task<byte[]> GetBlobAsync(string containerName, string blobName)
        {
            var blobServiceClient = new BlobServiceClient(_config.BlobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            using var resultStream = new MemoryStream();
            var blobStream = response.Value.Content;
            await blobStream.CopyToAsync(resultStream);

            return resultStream.ToArray();
        }
    }
}
