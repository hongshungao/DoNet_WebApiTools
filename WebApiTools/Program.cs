using AgileConfig.Client;

using Serilog;
using Serilog.Events;

using WebApiTools;
using WebApiTools.TestService;
using WebApiTools.Tools.Event_Bus;


try
{
    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
    Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Information()
      .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
      .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
      .Enrich.FromLogContext()
      .WriteTo.Logger(log => log.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information).WriteTo.File($"{baseDir}/Logs/{DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture) + "/info.txt"}", fileSizeLimitBytes: 8388608000, rollingInterval: RollingInterval.Day), LogEventLevel.Information)
      .WriteTo.Logger(log => log.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Warning).WriteTo.File($"{baseDir}/Logs/{DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture) + "/warning.txt"}", fileSizeLimitBytes: 83886080, rollingInterval: RollingInterval.Day), LogEventLevel.Warning)
      .WriteTo.Logger(log => log.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Fatal).WriteTo.File($"{baseDir}/Logs/{DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture) + "/fatal.txt"}", fileSizeLimitBytes: 83886080, rollingInterval: RollingInterval.Day), LogEventLevel.Fatal)
      .WriteTo.Logger(log => log.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error).WriteTo.File($"{baseDir}/Logs/{DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture) + "/error.txt"}", fileSizeLimitBytes: 83886080, rollingInterval: RollingInterval.Day), LogEventLevel.Error)
      .WriteTo.Async(c => c.Console())
      .CreateLogger();
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    //builder.Services.AddAgileConfig();
    #region ЗўЮёзЂВс
    ConfigureServices.Configure(builder);
    #endregion

    var app = builder.Build();
#if DEBUG
    app.UseSwagger();
    app.UseSwaggerUI();
#endif
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}
