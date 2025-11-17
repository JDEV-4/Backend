using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models.MetricsModels
{
    public class RequestMetric
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string EndpointPath { get; set; }
        public string HttpMethod { get; set; }
        public int StatusCode { get; set; }
        public long TiempoMs { get; set; }
    }
}
