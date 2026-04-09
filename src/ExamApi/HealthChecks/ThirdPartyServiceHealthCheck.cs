using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExamApi.HealthChecks;

public class ThirdPartyServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ThirdPartyServiceHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://httpbin.org/status/200", cancellationToken);

            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("第三方服务连接正常。")
                : HealthCheckResult.Unhealthy("第三方服务连接异常。");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("第三方服务连接失败。", ex);
        }
    }
}
