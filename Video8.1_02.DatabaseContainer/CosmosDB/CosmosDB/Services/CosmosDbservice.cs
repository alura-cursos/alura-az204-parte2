using Microsoft.Azure.Cosmos;

namespace CosmosDB.Services
{
    public class CosmosDbservice
    {
		private const string connectionString = "";
		private const string databaseId = "appcosmosdb";
		private const string containerId = "cursos";
		private const string partitionKey = "/categoria";

        public CosmosDbservice()
        {
			this.Database = CreateDatabaseAsync().Result;
			this.Container = CreateContainerAsync().Result;
        }

        private Database Database { get; set; }
		private Container Container { get; set; }

		public async Task<Database> CreateDatabaseAsync()
		{
			CosmosClient cosmosClient = new CosmosClient(connectionString);
			Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
			return database;
		}

		public async Task<Container> CreateContainerAsync()
		{
			Container container = await this.Database.CreateContainerIfNotExistsAsync(containerId, partitionKey);
			return container;
		}
	}
}
