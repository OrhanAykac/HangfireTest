using Hangfire;

namespace WebApp.Hangfire.CronJobs;

public class MinutelyJobs
{
    private readonly ILogger<MinutelyJobs> _logger;
    private readonly IRecurringJobManager _recurringJobManager;
    public MinutelyJobs(ILogger<MinutelyJobs> logger, IRecurringJobManager recurringJobManager)
    {
        _logger = logger;
        _recurringJobManager = recurringJobManager;
    }

    public void DoJobs()
    {
        _recurringJobManager.AddOrUpdate("Minutely Job", () => SayHello(), Cron.Minutely);

    }

    public void SayHello()
    {
        _logger.LogInformation("Hello from minutely schedule.");
    }
}
