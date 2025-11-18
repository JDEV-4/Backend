using Backend.Models.MetricsModels;
using Backend.Services.MetricsServices;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Backend.Middlewares
{
    public class RequestMetricsMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMetrics metrics)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);
            sw.Stop();

            string? user = context.User.Identity?.Name ?? "anonymous";

            var metric = new RequestMetric
            {
                EndpointPath = context.Request.Path,
                HttpMethod = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                TiempoMs = sw.ElapsedMilliseconds,
                UserName = user
            };

            _ = metrics.RecordEventAsync(metric); // No bloquea
        }
    }
}
