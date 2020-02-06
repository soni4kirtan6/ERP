using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace ConfigPortal.Pages
{
    public class MappingPageModel : PageModel
    {
        public string dbstruct { get; set; }
        public string[] Tables { get; set; }

        
        public MappingPageModel()
        {
            
        }
        public string ConStr { get; set; }
        public void OnGet()
        {
            //ConStr = "server='localhost';db='person';username='root';password=''";
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "DB_Structure.json";
            JObject dbstructure = JObject.Parse(System.IO.File.ReadAllText(path));
            dbstruct = dbstructure.ToString();
            int i = 0;
            Tables = new string[dbstructure.Properties().Count()];
            foreach (var table in dbstructure.Properties())
            {
                Tables[i++] = table.Name.ToString();
            }
        }
        
        
    }
}