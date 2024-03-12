
using BlackKiteTask.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog;
using Newtonsoft.Json.Linq;
using BlackKiteTask.Handlers;
using System.Reflection;
using BlackKiteTask.Common.Infrastructure;

#region Host Build
var host = new HostBuilder()
    #region Configuration
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureLogging((hostContext, loggingBuilder) =>
    {
        loggingBuilder.AddNLog(hostContext.Configuration);
    })
#endregion
    #region Dependency Injection
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient<IAuthorizationService, AuthorizationService>(
            client =>
            {
                client.BaseAddress = new Uri($"{hostContext.Configuration["BaseUrl"]}/oauth/");
            }
        );
        services.AddScoped<ITokenHandler, TokenHandler>().AddLogging();

        services.AddHttpClient<ICompanyService, CompanyService>(
            (provider,client) =>
            {
                var token = provider.GetRequiredService<ITokenHandler>().GetAccessToken().GetAwaiter().GetResult();  
                
                client.BaseAddress = new Uri($"{hostContext.Configuration["BaseUrl"]}/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        );

        services.AddScoped<IScanHandler, ScanHandler>();
    })
    #endregion
    .Build();
#endregion

//GetLogger
var logger = host.Services.GetService<ILogger<Program>>(); 
try
{
    //.\BlackKiteTask.exe <DOMAIN_NAME>
    //Start Scan
    await host.Services.GetRequiredService<IScanHandler>().Scan(args[0]);
}
#region Error Handling
catch (Exception ex)
{
    logger.LogErrorFormated(ex);
}
#endregion

