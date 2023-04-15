using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWorker
{
    public class ingr_sectorsSelect
    {  
        public string query(string value)
        {
           var query =  $"SELECT * FROM INGR_SECTORS where ATTR_NAME like '%{value}%'";
        
            return query.Replace(" ", "%20");
        }
        
    }
}