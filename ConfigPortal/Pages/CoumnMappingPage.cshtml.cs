using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string path1 = Environment.CurrentDirectory + "/OutputFiles/" + "table_mapping.json";
            JObject table_mapping = JObject.Parse(System.IO.File.ReadAllText(path1));

            JObject db_struct4dev=new JObject();
            JObject col_mapping =new JObject();
            foreach (var table in dbstructure.Properties())
            {
                JObject columns_obj = new JObject();
                JArray columns_array = new JArray();
                string c_Table = table.Name.ToString();
                foreach(var col in table.Value)
                {
                    string c_Column = col.ToString();
                    string AlternateName = Request.Form[c_Table+"-"+c_Column];
                    columns_obj.Add(AlternateName, c_Column);
                    columns_array.Add(AlternateName);
                }

                foreach (var x in table_mapping)
                {
                    if (x.Value.ToString().Equals(c_Table))
                    {
                        c_Table = x.Key.ToString();
                    }
                }
                col_mapping.Add(c_Table, columns_obj);//done
                db_struct4dev.Add(c_Table, columns_array);//done
            }

            
            //used only once
            Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");

            string path = Environment.CurrentDirectory + "/OutputFiles/" + "col_mapping.json";
            System.IO.File.WriteAllText(path, col_mapping.ToString());

            path = Environment.CurrentDirectory + "/OutputFiles/" + "DB_structure_forDev.json";
            System.IO.File.WriteAllText(path, db_struct4dev.ToString());

            return RedirectToPage("./ValidationPage");
        }
    }
}