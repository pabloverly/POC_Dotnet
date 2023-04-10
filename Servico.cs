using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ApiWorker;

namespace ApiWorker;

public class Servico : BackgroundService
{
    private const string Url = "https://dog.ceo/api/breeds/list/all";
    private readonly ILogger<Worker> _logger;

    public Servico(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public async Task<Repositorio> GetRepository(string owner, string repo)
    {   
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
            var response = await httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repo}");
            var json = await response.Content.ReadAsStringAsync();
            var repository = JsonConvert.DeserializeObject<Repositorio>(json);
            return repository;
        }
    }


   
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            var repository = await GetRepository("dotnet", "roslyn");
            Console.WriteLine(repository.Name);
            Console.WriteLine(repository.Description);
            Console.WriteLine(repository.Stars);
            Console.WriteLine(repository.Forks);                
                    
                        
            await Task.Delay(10000, stoppingToken);
        }
    }

    private void OnError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
        throw new NotImplementedException();
    }
}
