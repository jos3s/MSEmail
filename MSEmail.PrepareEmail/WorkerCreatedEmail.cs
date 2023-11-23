using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.PrepareEmail.Transaction;

namespace MSEmail.PrepareEmail
{
    public class WorkerCreatedEmail : BackgroundService
    {
        private readonly ILogger<WorkerCreatedEmail> _logger;

        private ExecuteCreatedEmailTRA _tra;
        private IServiceProvider _serviceProvider;

        public WorkerCreatedEmail(ILogger<WorkerCreatedEmail> logger, IServiceProvider serviceProvider, ExecuteCreatedEmailTRA executeCreatedEmailTRA)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _tra = executeCreatedEmailTRA;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (ConfigHelper.GetRunWorkerCreatedEmail())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var services = scope.ServiceProvider;
                    AppDbContext _context = services.GetService<AppDbContext>();
                    _tra.Execute(_context);
                    _logger.LogInformation("Worker Created Email running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
