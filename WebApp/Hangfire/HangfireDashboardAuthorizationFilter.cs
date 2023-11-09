using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WebApp.Hangfire;

public class HangfireDashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter
{
    public Task<bool> AuthorizeAsync([NotNull] DashboardContext context)
    {
        //var httpContext = context.GetHttpContext();
        //var userInRole = httpContext.User.IsInRole("CronJob.User");
        //return Task.FromResult(userInRole);
        return Task.FromResult(true);
    }
}
