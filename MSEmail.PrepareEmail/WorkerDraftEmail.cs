using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.PrepareEmail.Transaction;

namespace MSEmail.PrepareEmail
{
    public class WorkerDraftEmail : BackgroundService
    {
        private readonly ILogger<WorkerCreatedEmail> _logger;
        private IServiceProvider _serviceProvider;
        private ExecuteDraftEmailTRA _tra;

        public WorkerDraftEmail(ILogger<WorkerCreatedEmail> logger, IServiceProvider serviceProvider, ExecuteDraftEmailTRA executeDraftEmailTRA)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _tra = executeDraftEmailTRA;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (ConfigHelper.GetRunWorkerDraftEmail())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var services = scope.ServiceProvider;
                    AppDbContext _context = services.GetService<AppDbContext>();
                    _tra.Execute(_context);
                    _logger.LogInformation("Worker Draft Email running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
