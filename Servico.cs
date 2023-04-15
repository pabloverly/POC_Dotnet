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
 
   
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
             using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
                var response = await httpClient.GetAsync($"https://api2.instarsuite.com/brasil_syndicated/IADS.asmx/GetData?iads_params=name%3AAPI_GazetaES%3Bpassword%3Ac8U0pK1wuK%3BidLang%3AEN%3BidApp%3A3000%3Boutformat%3ACSV&tqx=responseHandler%3AhandleTqResponse&tq=SELECT%20*%20FROM%20%09INGR_SECTORS");
                
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var content = await response.Content.ReadAsStringAsync();

                   
                    
                    var rows = content.Split('\n');

                     //Console.WriteLine(rows[3]);

                    for(int i = 3; i < rows.Length; i++)
                    {
                        // Console.WriteLine(rows[i]);
                        //rows[i] = rows[i].Replace("\"", "");
                        var columns = rows[i].Split(';');
                        Console.WriteLine(columns[0] + " " + columns[1]);     

                        var connectionString = "Data Source=192.168.1.65; TrustServerCertificate=True; MultiSubnetFailover=True; Initial Catalog=master; User ID='sa'; Password='Pv020502@'";
                        using var connection = new SqlConnection(connectionString);
                        connection.Open();  


                        using var transaction = connection.BeginTransaction();
                            

                        var commandText = "INSERT INTO INGR_SECTORS (ATTR_ID, ATTR_NAME) VALUES (@ATTR_ID, @ATTR_NAME )";
                        using var command = new SqlCommand(commandText, connection, transaction);

                        command.Parameters.AddWithValue("@ATTR_ID", columns[0]);
                        command.Parameters.AddWithValue("@ATTR_NAME", columns[1]);                         

                        await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                        connection.Close();
                                  
                    }

                    // foreach (var row in rows)
                    // {
                    //     var columns = row.Split(';');

                    //     Console.WriteLine(columns[1]);

                        
                    //     // Console.WriteLine(columns[1]);
                 
                    //     //console.writeline(columns[0]);
                    // }
                
                
                }
            
             
            }             
                    
                        
            await Task.Delay((1 * 60) * 1000 , stoppingToken);
        }
    }

    private void OnError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
        throw new NotImplementedException();
    }
}
