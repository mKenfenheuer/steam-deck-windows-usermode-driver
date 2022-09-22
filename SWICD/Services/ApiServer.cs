using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger;

namespace SWICD.Services
{
    internal class ApiServer
    {
        private static ApiServer _instance;
        public static ApiServer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ApiServer();
                return _instance;
            }
        }


        public void StartServer()
        {
            var builder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder();

            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureAppConfiguration(ConfigureAppConfig);
            builder.Configure(ConfigureApp);
            builder.ConfigureKestrel(ConfigureKestrel);
            builder.ConfigureLogging(ConfigureLogging);

            var app = builder.Build();

            app.Start();
        }

        private void ConfigureLogging(ILoggingBuilder b)
        {
            b.ClearProviders();
            b.AddProvider(new CustomLoggerProvider());
            //b.AddConsole();
        }

        private void ConfigureKestrel(KestrelServerOptions options)
        {
            options.ListenLocalhost(9373);
        }

        private void ConfigureApp(IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SWICD API");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Api}/{action=Index}/{id?}");
            });
        }

        private void ConfigureAppConfig(WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder configurationBuilder)
        {

        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SWICD API", Version = BuildVersionInfo.Version});
            });
            services.AddMvc();
        }
    }

    internal class CustomLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    LoggingService.LogDebug(formatter(state, exception));
                    break;
                case LogLevel.Information:
                default:
                    LoggingService.LogInformation(formatter(state, exception));
                    break;
                case LogLevel.Warning:
                    LoggingService.LogWarning(formatter(state, exception));
                    break;
                case LogLevel.Error:
                    LoggingService.LogError(formatter(state, exception));
                    break;
                case LogLevel.Critical:
                    LoggingService.LogCritical(formatter(state, exception));
                    break;
            }
        }
    }
    public sealed class CustomLoggerProvider : ILoggerProvider
    {
        private CustomLogger _logger;

        public CustomLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_logger == null)
                _logger = new CustomLogger();
            return _logger;
        }


        public void Dispose()
        {

        }
    }
}
