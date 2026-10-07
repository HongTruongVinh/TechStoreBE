using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.UnitOfWork;

namespace TechStore.Service.Background
{
    public class IdempotencyKeyCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<IdempotencyKeyCleanupService> _logger;

        public IdempotencyKeyCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<IdempotencyKeyCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var unitOfWork = scope.ServiceProvider
                        .GetRequiredService<IUnitOfWork>();

                    await unitOfWork.IdempotencyKeys.DeleteExpiredAsync(DateTimeOffset.UtcNow, stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error while cleaning expired idempotency keys.");
                }
            }
        }
    }
}
