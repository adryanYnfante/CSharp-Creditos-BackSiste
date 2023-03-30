using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Creditos.AppServices.Messaging.Extensions.Health
{
    public static class HealthCheckExtensions
    {
        public static IApplicationBuilder ServiceHealthChecks(this IApplicationBuilder app, string endpoint, string serviceName)
        {
            return app.UseHealthChecks(endpoint, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    string result = JsonSerializer.Serialize(
                    new HealthResult
                    {
                        ServiceName = serviceName,
                        Status = report.Status.ToString(),
                        Duration = report.TotalDuration,
                        Checks = report.Entries.Select(e => new HealthInfo
                        {
                            Name = e.Key,
                            Description = e.Value.Description,
                            Duration = e.Value.Duration,
                            Status = Enum.GetName(typeof(HealthStatus),
                                                    e.Value.Status),
                            Error = e.Value.Exception?.Message
                        }).ToList()
                    },
                    new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        WriteIndented = false
                    });

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });
        }
    }
}