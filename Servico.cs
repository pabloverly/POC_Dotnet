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

   public async Task GetRepositoryAndInsertToSQLServer(string owner, string repo, string connectionString)
 {   
        
    using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
            var response = await httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repo}");
            var json = await response.Content.ReadAsStringAsync();
            var repository = JsonConvert.DeserializeObject<Repositorio>(json);

            using (var connection = new SqlConnection(connectionString))
            {
                var commandText = "INSERT INTO Repositories (Name, Description, Stars, Forks) " +
                                "VALUES (@Name, @Description, @Stars, @Forks)";
                var command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Name", repository.Name);
                command.Parameters.AddWithValue("@Description", repository.Description);
                command.Parameters.AddWithValue("@Stars", repository.Stars);
                command.Parameters.AddWithValue("@Forks", repository.Forks);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


   
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var connectionString = "Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;";
            await GetRepositoryAndInsertToSQLServer("dotnet", "roslyn", connectionString);

                
                    
                        
            await Task.Delay(10000, stoppingToken);
        }
    }

    private void OnError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
        throw new NotImplementedException();
    }
}
