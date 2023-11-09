using Hangfire;
using Serilog;

namespace WebApp.Hangfire.CronJobs;

public class DailyJobs
{
    private readonly ILogger<DailyJobs> _logger;
    private readonly IRecurringJobManager _recurringJobManager;
    public DailyJobs( ILogger<DailyJobs> logger, IRecurringJobManager recurringJobManager)
    {
        _logger = logger;
        _recurringJobManager = recurringJobManager;
    }


    public void DoJobs()
    {
        _recurringJobManager.AddOrUpdate("Daily Job", () => SayHello(), Cron.Daily);

    }

    public void SayHello()
    {
        _logger.LogWarning("Hello from daily schedule.");
    }
}
