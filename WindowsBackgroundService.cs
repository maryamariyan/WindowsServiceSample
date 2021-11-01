using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace App.WindowsService
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly WindowsServiceLifetime? _serviceLifetime;
        private readonly ILogger<WindowsBackgroundService> _logger;

        public WindowsBackgroundService(
            IEnumerable<IHostLifetime> serviceLifetimes,
            ILogger<WindowsBackgroundService> logger)
        {
            _logger = logger;

            // This will only be available when actually running as a Windows Service.
            _serviceLifetime =
                serviceLifetimes.OfType<WindowsServiceLifetime>()
                    .Cast<WindowsServiceLifetime>()
                    .SingleOrDefault();

            if (_serviceLifetime is not null)
            {
                _logger.LogWarning("ServiceName = {ServiceName}", _serviceLifetime.ServiceName);
                _logger.LogWarning("AutoLog = {AutoLog}", _serviceLifetime.AutoLog);
                _logger.LogWarning("CanShutdown = {CanShutdown}", _serviceLifetime.CanShutdown);
                _logger.LogWarning("CanHandleSessionChangeEvent = {CanHandleSessionChangeEvent}", _serviceLifetime.CanHandleSessionChangeEvent);
                _logger.LogWarning("CanPauseAndContinue = {CanPauseAndContinue}", _serviceLifetime.CanPauseAndContinue);
                _logger.LogWarning("CanStop = {CanStop}", _serviceLifetime.CanStop);
                _logger.LogWarning("ExitCode = {ExitCode}", _serviceLifetime.ExitCode);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_serviceLifetime is not null)
                {
                    _logger.LogWarning(
                        "ExecuteAsync: {ServiceName}", _serviceLifetime.ServiceName);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (_serviceLifetime is not null)
            {
                _logger.LogWarning("StartAsync: {ServiceName}", _serviceLifetime.ServiceName);
            }

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            if (_serviceLifetime is not null)
            {
                _logger.LogWarning("StopAsync: {ServiceName}", _serviceLifetime.ServiceName);
            }

            return base.StopAsync(cancellationToken);
        }
    }
}
