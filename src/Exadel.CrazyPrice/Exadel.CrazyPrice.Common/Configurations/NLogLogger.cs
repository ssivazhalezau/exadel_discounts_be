﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Exadel.CrazyPrice.Common.Configurations
{
    public class NLogLogger : ICrazyPriceLogger
    {
        private Logger _logger;
        private readonly IHostBuilder _hostBuilder;
        private readonly IConfigurationRoot _configuration;

        public NLogLogger(IHostBuilder hostBuilder, IConfigurationRoot configuration)
        {
            _hostBuilder = hostBuilder;
            _configuration = configuration;
        }

        public IHostBuilder UseLogger()
        {
            LogManager.Configuration = new NLogLoggingConfiguration(_configuration.GetSection("NLog"));
            _logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

            return _hostBuilder
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.SetMinimumLevel(LogLevel.Trace);
                        })
                        .UseNLog();
        }

        public void HostRun(string initMessage, string errorInitMessage)
        {
            try
            {
                _logger.Info(initMessage);
                _hostBuilder.Build().Run();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, errorInitMessage);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
