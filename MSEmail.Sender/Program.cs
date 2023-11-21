using Microsoft.EntityFrameworkCore;
using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.Sender;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        ConfigurationAppSettings.ConfigureSettings(context.Configuration);
        services.AddDbContextPool<AppDbContext>(
            db => db.UseSqlServer(context.Configuration.GetConnectionString("MsEmail"),
            b => b.MigrationsAssembly("MSEmail.Infra")
        ));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
