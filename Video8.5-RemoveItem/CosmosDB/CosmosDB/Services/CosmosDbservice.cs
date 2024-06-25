using CosmosDB.Models;
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

		public async Task AddNewItemAsync(Curso curso)
		{
			ItemResponse<Curso> itemResponse = await this.Container.CreateItemAsync(curso, new PartitionKey(curso.categoria));
		}

		public async Task<IEnumerable<Curso>> GetAllItemsAsync()
		{
			QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM cursos");
			List<Curso> cursos = new List<Curso>();

			using (FeedIterator<Curso> feedIterator = this.Container.GetItemQueryIterator<Curso>(queryDefinition))
			{
				while (feedIterator.HasMoreResults)
				{
					FeedResponse<Curso> response = await feedIterator.ReadNextAsync();
					foreach (var item in response)
					{
						cursos.Add(item);
					}
				}
			}
			return cursos;
		}

		public async Task<Curso> FindItemAsync(string id, string categoria)
		{
			ItemResponse<Curso> response = await this.Container.ReadItemAsync<Curso>(id, new PartitionKey(categoria));
			return response.Resource;
		}

		public async Task UpdateItemAsync(Curso curso)
		{
			await this.Container.PatchItemAsync<Curso>(
				id: curso.id,
				partitionKey: new PartitionKey(curso.categoria),
				patchOperations: new[]{
					PatchOperation.Replace("/descricao", curso.descricao),
					PatchOperation.Replace("/avaliacao", curso.avaliacao)
				});
		}

		public async Task RemoveItemAsync(string id, string categoria)
		{
			await this.Container.DeleteItemAsync<Curso>(id, new PartitionKey(categoria));
		}
	}
}
