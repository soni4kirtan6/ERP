using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace ConfigPortal.Pages
{
    public class CoumnMappingPageModel : PageModel
    {
        public JObject dbstructure;
        public CoumnMappingPageModel()
        {
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "DB_Structure.json";
            dbstructure = JObject.Parse(System.IO.File.ReadAllText(path));

        }
        public void OnGet()
        {
            
           
        }
    }
}