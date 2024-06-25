namespace AzureBlobStorage.Services
{
    public class BlobsService
    {
        public IConfiguration Configuration { get; set; }
        public BlobsService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

    }
}
