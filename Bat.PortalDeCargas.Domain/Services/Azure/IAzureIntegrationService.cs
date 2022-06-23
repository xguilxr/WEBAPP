using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Azure
{
    public interface IAzureIntegrationService
    {
        Task PostBlobAsync(byte[] fileContent, Entities.Template template);

        Task<byte[]> GetBlobAsync(string containerName, string blobName);
    }
}
