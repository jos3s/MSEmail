using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.Infra.Log;
using MSEmail.PrepareEmail.Transaction;

namespace MSEmail.PrepareEmail;

public class WorkerDraftEmail : BackgroundService
{
    private readonly ILogger<WorkerCreatedEmail> _logger;
    private IServiceProvider _serviceProvider;

    public WorkerDraftEmail(ILogger<WorkerCreatedEmail> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (ConfigHelper.GetRunWorkerDraftEmail)
            {
                _logger.LogInformation("Worker Draft Email running at: {time}", DateTimeOffset.Now);
                    
                #region context
                using var scope = _serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                AppDbContext _context = services.GetService<AppDbContext>();
                #endregion

                new ExecuteTRA(_context).Execute(Domain.Enums.EmailStatus.Draft);
                await Task.Delay(TimeSpan.FromMinutes(ConfigHelper.ServiceInterval), stoppingToken);

                LogSingleton.Instance.CreateInformationLog($"{nameof(WorkerDraftEmail)}.{nameof(ExecuteAsync)}", "", Domain.Enums.ServiceType.MSSender);
            }
        }
    }
}