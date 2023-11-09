using Hangfire;

namespace WebApp.Hangfire.CronJobs;

public class HourlyJobs
{
    private readonly ILogger<HourlyJobs> _logger;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    public HourlyJobs(IBackgroundJobClient backgroundJobClient, ILogger<HourlyJobs> logger, IRecurringJobManager recurringJobManager)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
        _recurringJobManager = recurringJobManager;
    }

    public void DoJobs()
    {
        _recurringJobManager.AddOrUpdate("Hourly Job", () => SayHello(), Cron.Hourly);
        _backgroundJobClient.Schedule(() =>
        SayHello(),
        TimeSpan.FromHours(1));
    }

    public void SayHello()
    {
        _logger.LogInformation("Hello from hourly schedule.");
    }
}
