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
    public class ValidationPageModel : PageModel
    {
        public JObject dbstructure;
        public ValidationPageModel()
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
            int code = 1001;
            JObject val_config = new JObject();
            foreach (var table in dbstructure.Properties())
            {
                JObject columns = new JObject();
                string c_Table = table.Name.ToString();
                foreach (var col in table.Value)
                {
                    JArray val_arr = new JArray();
                    string c_Column = col.ToString();
                    string count = Request.Form["count-"+c_Table + "-" + c_Column];
                    //int length= (int)Request.Form[];
                    
                    for (int i = 0; i < Convert.ToInt32(count); i++)
                    {
                        JObject val_obj = new JObject();
                        //string val_no= Request.Form["val_no-" + c_Table + "-" + c_Column+"-"+i];
                        string type_sel = Request.Form["type_sel-" + c_Table + "-" + c_Column + "-" + i];
                        string value = Request.Form["value-" + c_Table + "-" + c_Column + "-" + i];
                        string E_W = Request.Form["E_W-" + c_Table + "-" + c_Column + "-" + i];

                        val_obj.Add("typeKey",type_sel);
                        val_obj.Add("keyValue", value);
                        val_obj.Add("errorOrWarning", E_W);
                        val_obj.Add("MsgTextNo", (code++).ToString());//doubt individual val or val type has unique no
                        //code++;
                        val_arr.Add(val_obj);
                    }

                    columns.Add(c_Column, val_arr);
                }
                val_config.Add(c_Table, columns);
            }



            Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "validationConfig.json";


            System.IO.File.WriteAllText(path, val_config.ToString());
            return RedirectToPage("./AuthenticationPage");
        }
    }
}