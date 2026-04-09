using ExamApi.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExamApi.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly AppDbContext _dbContext;

    public DatabaseHealthCheck(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("数据库连接正常。")
                : HealthCheckResult.Unhealthy("数据库连接失败。");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("数据库连接异常。", ex);
        }
    }
}
