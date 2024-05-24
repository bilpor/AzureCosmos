using AzureCosmos.Data;
using Microsoft.Azure.Cosmos;

namespace AzureCosmos.Services
{
    public interface IEngineerService
    {
        Task DeleteEngineer(string? id, string? partitionKey);
        Task<Container> GetContainerClient();
        Task<List<Engineer>> GetEngineerDetails();
        Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey);
        Task UpsertEngineer(Engineer engineer);
    }
}