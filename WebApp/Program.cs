
using Hangfire;
using Hangfire.MemoryStorage;
using Serilog;
using Serilog.Events;
using WebApp;
using WebApp.Hangfire;
using WebApp.Hangfire.CronJobs;

//serilog config
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//inmemory implementation
builder.Services.AddMemoryCache();


builder.Services.AddSingleton<DailyJobs>();
builder.Services.AddSingleton<HourlyJobs>();
builder.Services.AddSingleton<MinutelyJobs>();

//build hangfire
builder.Services.AddHangfire(hangfire =>
{
    hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
    hangfire.UseSimpleAssemblyNameTypeSerializer();
    hangfire.UseRecommendedSerializerSettings();
    hangfire.UseColouredConsoleLogProvider();
    hangfire.UseMemoryStorage();
    #region sql config DISABLED
    //hangfire.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnTest"),
    //    new SqlServerStorageOptions
    //    {
    //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    //        QueuePollInterval = TimeSpan.Zero,
    //        UseRecommendedIsolationLevel = true,
    //        DisableGlobalLocks = true
    //    });
    #endregion
});

builder.Services.AddHangfireServer();



builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseHangfireDashboard("/hang-fire", new DashboardOptions()
{
    DarkModeEnabled = true,
    DashboardTitle = "Hangfire Test",
    AsyncAuthorization = new[] { new HangfireDashboardAuthorizationFilter() }
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

CallJobs();

app.Run();


void CallJobs()
{
    var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<MinutelyJobs>().DoJobs();
    scope.ServiceProvider.GetRequiredService<DailyJobs>().DoJobs();
    scope.ServiceProvider.GetRequiredService<HourlyJobs>().DoJobs();
}