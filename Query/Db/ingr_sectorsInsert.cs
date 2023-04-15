using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POC_Dotnet.Data;

namespace ApiWorker
{
    public class ingr_sectorsInsert
    {  
        public async  void  insert(string param1,string param2 )
        {
            try{
                         
                            var connectionString = "Data Source=192.168.1.65; TrustServerCertificate=True; MultiSubnetFailover=True; Initial Catalog=master; User ID='sa'; Password='Pv020502@'";
                            using var connection = new SqlConnection(connectionString);
                            connection.Open();  
                            
                          

                            using var transaction = connection.BeginTransaction();                            

                            var commandText = "INSERT INTO INGR_SECTORS (ATTR_ID, ATTR_NAME) VALUES (@ATTR_ID, @ATTR_NAME )";
                            using var command = new SqlCommand(commandText, connection, transaction);

                            command.Parameters.AddWithValue("@ATTR_ID", param1);
                            command.Parameters.AddWithValue("@ATTR_NAME", param2);                         

                            await command.ExecuteNonQueryAsync();
                            transaction.Commit();

                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }           
        }
        
        
    }
}