using AzureCosmos.Data;
using Microsoft.Azure.Cosmos;

namespace AzureCosmos.Services;

public class EngineerService : IEngineerService
{
    private readonly string CosmosDBAccountUri = "https://localhost:8081";
    private readonly string CosmosDBAccountPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    private readonly string CosmosDbName = "Contractors";
    private readonly string CosmosDbContainerName = "Engineers";
    private readonly string CosmosDbConectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";


    public async Task<Container> GetContainerClient()
    {
        try
        {
            var cosmosDBClient = new CosmosClient(CosmosDbConectionString);


            Database database = await cosmosDBClient.CreateDatabaseIfNotExistsAsync(
                id: "Contractors",
                throughput: 400
            );

            Container dbContainer = await database.CreateContainerIfNotExistsAsync(
                id: "Engineers",
                partitionKeyPath: "/id",
                throughput: 400
            );

            var container = cosmosDBClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return container;
        }
        catch (Exception ex)
        {

            throw;
        }
        
    }

    public async Task UpsertEngineer(Engineer engineer)
    {
        try
        {

            if (engineer.id == null)
            {
                engineer.id = Guid.NewGuid();
            }
            var container = await GetContainerClient();
            var response = await container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
            Console.WriteLine(response.StatusCode);
        }
        catch (Exception ex)
        {

            throw new Exception("Exception", ex);
        }
    }

    public async Task DeleteEngineer(string? id, string? partitionKey)
    {
        try
        {
            var container = await GetContainerClient();
            var response = await container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
            Console.WriteLine(response.StatusCode);
        }
        catch (Exception ex)
        {

            throw new Exception("Exception ", ex);
        }
    }

    public async Task<List<Engineer>> GetEngineerDetails()
    {
        List<Engineer> engineers = new List<Engineer>();
        try
        {
            var container = await GetContainerClient();
            var sqlQuery = "Select * From c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
            FeedIterator<Engineer> queryResultSetIterator = container.GetItemQueryIterator<Engineer>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Engineer engineer in currentResultSet)
                {
                    engineers.Add(engineer);
                }
            }
        }
        catch (Exception ex)
        {

            ex.Message.ToString();
        }
        return engineers;
    }

    public async Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey)
    {
        try
        {
            var container = await GetContainerClient();
            ItemResponse<Engineer> response = await container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
            return response.Resource;
        }
        catch (Exception ex)
        {

            throw new Exception("Exception ", ex);
        }
    }
}
