using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models.MetricsModels
{
    public class UserActivityMetric
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Métrica principal
        public string EventType { get; set; } 

        // Tags para análisis
        public string UserName { get; set; }
        public string EndpointPath { get; set; }
    }
}
