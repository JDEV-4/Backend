using Backend.Services.MetricsServices;
using MongoDB.Driver;

public class MetricService : IMetrics
{
    private readonly IMongoDatabase _database;

    public MetricService(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("MongoDbConnection");
        string databaseName = configuration.GetValue<string>("MongoSettings:DatabaseName");

        var settings = MongoClientSettings.FromConnectionString(connectionString);

        // Mejor rendimiento (pide tu docente)
        settings.MaxConnectionPoolSize = 100;
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        settings.ConnectTimeout = TimeSpan.FromSeconds(5);

        var client = new MongoClient(settings);
        _database = client.GetDatabase(databaseName);
    }

    public async Task RecordEventAsync<TDocument>(TDocument metric) where TDocument : class
    {
        string collectionName = $"{typeof(TDocument).Name}s"; // Plural
        var collection = _database.GetCollection<TDocument>(collectionName);

        await collection.InsertOneAsync(metric);
    }
}
