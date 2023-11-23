using Microsoft.EntityFrameworkCore;
using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.PrepareEmail;
using MSEmail.PrepareEmail.Transaction;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        ConfigurationAppSettings.ConfigureSettings(context.Configuration);
        services.AddDbContextPool<AppDbContext>(
            db => db.UseSqlServer(context.Configuration.GetConnectionString("MsEmail"),
            b => b.MigrationsAssembly("MSEmail.Infra")
        ));

        if (ConfigHelper.GetRunWorkerCreatedEmail())
            services.AddHostedService<WorkerCreatedEmail>();

        if (ConfigHelper.GetRunWorkerDraftEmail())
            services.AddHostedService<WorkerDraftEmail>();
    })
    .Build();

await host.RunAsync();
