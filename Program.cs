using ApiWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Servico>();
    })
    .Build();

await host.RunAsync();
