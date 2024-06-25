using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace AzureBlobStorage.Services
{
    public class BlobsService
    {
        public IConfiguration Configuration { get; set; }
        public BlobsService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public async Task<BlobContainerClient> GetBlobContainerAsync(string containerName)
        {
            var conexao = Configuration.GetConnectionString("BlobConnectionString");
            BlobServiceClient serviceClient = new BlobServiceClient(conexao);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(containerName);
            
            await containerClient.CreateIfNotExistsAsync();

            return containerClient;
        }

        public async Task UploadBlobAsync(string blob)
        {
            var container = Configuration["BlobContainerName"];
            BlobContainerClient containerClient = await GetBlobContainerAsync(container!);
            BlobClient blobClient = containerClient.GetBlobClient(blob);
            await blobClient.UploadAsync($"D:/Azure/imagens/{blob}");
        }

        public async Task<List<string>> GetAllBlobs()
        {
			var conexao = Configuration.GetConnectionString("BlobConnectionString");
			BlobServiceClient serviceClient = new BlobServiceClient(conexao);

			var container = Configuration["BlobContainerName"];
			BlobContainerClient containerClient = await GetBlobContainerAsync(container!);

            List<string> results = new List<string>();

			if (await containerClient.ExistsAsync())
			{
				BlobClient blobClient;
				BlobSasBuilder blobSasBuilder;
				await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
				{
					blobClient = containerClient.GetBlobClient(blobItem.Name);

					blobSasBuilder = new BlobSasBuilder()
					{
						BlobContainerName = containerClient.Name,
						BlobName = blobItem.Name,
						ExpiresOn = DateTime.UtcNow.AddMinutes(5),
						Protocol = SasProtocol.Https
					};
					blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

					//results.Add(blobClient.Uri.ToString());
					results.Add(blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri);
				}
			}
			return results;
		}
    }
}
