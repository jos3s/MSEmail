using MsEmail.Infra.Context;
using MSEmail.Sender.Transaction;

namespace MSEmail.Sender;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            AppDbContext _context = services.GetService<AppDbContext>();
            new ExecuteTRA(_context).Execute();
        }
    }
}
