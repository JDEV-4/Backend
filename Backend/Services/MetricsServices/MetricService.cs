using Backend.Services.MetricsServices;
using MongoDB.Driver;

public class MetricService : IMetrics
{
    private readonly IMongoDatabase _mongoDatabase;

    public MetricService(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("MongoDbConnection");
        string databaseName = configuration.GetValue<string>("MongoSettings:DatabaseName");

        var mongoClient = new MongoClient(connectionString);
        _mongoDatabase = mongoClient.GetDatabase(databaseName);
    }

    public async Task RecordEventAsync<TDocument>(TDocument metric) where TDocument : class
    {
        var collectionName = typeof(TDocument).Name;
        var collection = _mongoDatabase.GetCollection<TDocument>(collectionName);
        await collection.InsertOneAsync(metric);
    }
}
