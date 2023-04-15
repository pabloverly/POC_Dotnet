using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ApiWorker;


namespace ApiWorker;

public class Servico : BackgroundService
{
    private const string Url = "https://dog.ceo/api/breeds/list/all";
    private readonly ILogger<Worker> _logger;
    private readonly Api _api;

    public Servico(ILogger<Worker> logger , IConfiguration _configuration)
    {
        _logger = logger;
        _api = _configuration.GetSection("ApiUrl").Get<Api>(); 
    }
 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
             using (var httpClient = new HttpClient())
            {
    
               var select = new ingr_sectorsSelect();

               //Console.WriteLine(sql);
                //httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
               // var response = await httpClient.GetAsync($"https://api2.instarsuite.com/brasil_syndicated/IADS.asmx/GetData?iads_params=name%3AAPI_GazetaES%3Bpassword%3Ac8U0pK1wuK%3BidLang%3AEN%3BidApp%3A3000%3Boutformat%3ACSV&tq="+ select.query("Grande"));
                var response = await httpClient.GetAsync($"https://api2.instarsuite.com/brasil_syndicated/IADS.asmx/GetData?iads_params=name%3AAPI_GazetaES%3Bpassword%3Ac8U0pK1wuK%3BidLang%3AEN%3BidApp%3A3000%3Boutformat%3ACSV&tq="+ select.query("Grande"));
                
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var content = await response.Content.ReadAsStringAsync();                   
                    
                    var rows = content.Split('\n');                   

                     for(int i = 3; i < rows.Length; i++)
                    {
                        // Console.WriteLine(rows[i]);
                        //rows[i] = rows[i].Replace("\"", "");
                        var columns = rows[i].Split(';');
                        Console.WriteLine(columns[0] + " " + columns[1]);     

                        var insert = new ingr_sectorsInsert();
                        insert.insert(columns[0], columns[1]);
                   
                    }    
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
