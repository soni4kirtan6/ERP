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
        public string DbStruct { get; set; }
        public string[] Tables { get; set; }
        public string[] AlternateName { get; set; }
        
        public string ConStr { get; set; }
        public MappingPageModel()
        {
            //ConStr = "server='localhost';db='person';username='root';password=''";
                string path = Environment.CurrentDirectory + "/OutputFiles/" + "DB_Structure.json";
            JObject dbstructure = JObject.Parse(System.IO.File.ReadAllText(path));
            DbStruct = dbstructure.ToString();
            int i = 0;
            Tables = new string[dbstructure.Properties().Count()];
            AlternateName = new string[dbstructure.Properties().Count()];
            foreach (var table in dbstructure.Properties())
            {
                Tables[i++] = table.Name.ToString();
            }
        }
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //
            if (AlternateName.Length==0)
            {
                return Page();
            }
            for (int i = 0; i < AlternateName.Length; i++)
            {
                AlternateName[i] = Request.Form[nameof(AlternateName)+"["+i+"]"];
            }
            JObject table_mapping = new JObject();
            for (int i = 0; i < Tables.Length; i++)
            {
                table_mapping.Add(AlternateName[i],Tables[i]);
            }

                Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");
                string path = Environment.CurrentDirectory + "/OutputFiles/" + "table_mapping.json";


            System.IO.File.WriteAllText(path, table_mapping.ToString());
            return RedirectToPage("./CoumnMappingPage");
        }


    }
}