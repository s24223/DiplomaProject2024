using Application.Databases.NonRelational;
using Infrastructure.Exceptions.AppExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Infrastructure.Databases.NonRelational
{
    public class MongoDbRepository : NonRelationalDbRepository
    {
        //Properties
        private readonly string _connectionString;
        private readonly string _dbName;


        //Constructor
        public MongoDbRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("MongoDb")["DbString"] ??
                throw new InfrastructureLayerException(Messages.NotConfiguredConnectionString);
            _dbName = configuration.GetSection("MongoDb")["DbName"] ??
                throw new InfrastructureLayerException(Messages.NotConfiguredConnectionString);
        }

        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods

        public async Task<string> SaveAsync(IFormFile file, CancellationToken cancellation)
        {
            // Połączenie z MongoDB
            using (var client = new MongoClient(_connectionString))
            {
                var database = client.GetDatabase(_dbName);
                // Tworzenie obiektu GridFS
                var gridFS = new GridFSBucket(database);

                await using var stream = file.OpenReadStream();
                var fileId = await gridFS.UploadFromStreamAsync(
                    file.FileName,
                    stream,
                    new GridFSUploadOptions
                    {
                        Metadata = new MongoDB.Bson.BsonDocument
                        {
                        { "ContentType", file.ContentType },
                        { "UploadedAt", DateTime.UtcNow }
                        }
                    }
                );
                return fileId.ToString();
            }
        }

        public async Task<(MemoryStream Stream, string Name)?> GetAsync(string id, CancellationToken cancellation)
        {
            // Połączenie z MongoDB
            using (var client = new MongoClient(_connectionString))
            {
                var database = client.GetDatabase(_dbName);
                // Tworzenie obiektu GridFS
                var gridFS = new GridFSBucket(database);

                try
                {
                    var objectId = new ObjectId(id);
                    var fileStream = new MemoryStream();

                    // Pobieranie pliku z MongoDB do MemoryStream
                    await gridFS.DownloadToStreamAsync(objectId, fileStream, cancellationToken: cancellation);

                    fileStream.Position = 0; // Ustawienie pozycji na początek strumienia

                    var fileName = id; // Możesz ustawić nazwę pliku, jeśli jest dostępna w bazie

                    var encodedFileName = Uri.EscapeDataString(fileName);

                    fileStream.Position = 0;
                    return (fileStream, encodedFileName);
                }
                catch (GridFSFileNotFoundException)
                {
                    return null;
                }
            }
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
    }
}
