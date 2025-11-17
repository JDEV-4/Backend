using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models.MetricsModels
{
    public class TransactionMetric
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string TransactionName { get; set; }
        public string UserId { get; set; }
        public bool Success { get; set; }
        public decimal Amount { get; set; }
        public int ItemsCount { get; set; }
    }
}
