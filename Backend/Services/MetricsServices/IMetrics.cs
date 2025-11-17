namespace Backend.Services.MetricsServices
{
    public interface IMetrics
    {
        Task RecordEventAsync<TDocument>(TDocument metric) where TDocument : class;
    }
}
