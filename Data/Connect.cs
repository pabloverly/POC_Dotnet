using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace POC_Dotnet.Data
{
    public class Connect
    {
        public void open(){
            var connectionString = "Data Source=192.168.1.65; TrustServerCertificate=True; MultiSubnetFailover=True; Initial Catalog=master; User ID='sa'; Password='Pv020502@'";
            using var connection = new SqlConnection(connectionString);
            connection.Open();  
        }
        public void close(){

        }

    }
}