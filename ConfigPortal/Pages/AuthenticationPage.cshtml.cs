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
    public class AuthenticationPageModel : PageModel
    {
        public JObject dbstructure;

        public AuthenticationPageModel()
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


            string path = Environment.CurrentDirectory + "/OutputFiles/" + "authentication.json";
            JObject authentication_json = JObject.Parse(System.IO.File.ReadAllText(path));

            string Role = Request.Form["role_name"];
            JObject table_obj = new JObject();
            foreach (var table in dbstructure.Properties())
            {
                JObject table_sub_obj = new JObject();
                string TableName = table.Name.ToString();


                string[] crud = { "-","-","-","-"};
                string c = Request.Form["c-"+TableName];
                string r = Request.Form["r-" + TableName];
                string u = Request.Form["u-" + TableName];
                string d = Request.Form["d-" + TableName];
                if (c.Equals("c"))crud[0] = "C";
                if (r.Equals("r"))crud[1] = "R";
                if (u.Equals("u"))crud[2] = "U";
                if (d.Equals("d"))crud[3] = "D";

                string[] readArray;
                string[] writeArray;

               
                readArray= Request.Form["readvalues-"+TableName];
                writeArray= Request.Form["writevalues-" + TableName];

                JArray not_readArray = new JArray();
                JArray not_writeArray = new JArray();

                foreach (var col in table.Value)
                {
                    if (!readArray.Contains(col.ToString()))
                    {
                        not_readArray.Add(col.ToString());
                    }
                    if (!writeArray.Contains(col.ToString()))
                    {
                        not_readArray.Add(col.ToString());
                    }
                }
                table_sub_obj.Add("CRUD",crud.ToString());

                //add inverted columns
                table_sub_obj.Add("!ReadColumns", not_readArray);
                table_sub_obj.Add("!WriteColumns",not_writeArray);

                //add table only if one of crud is selected
                table_obj.Add(TableName,table_sub_obj);
            }
                authentication_json.Add(Role, table_obj);
            //Think about how to initialize the json first time configuring



            Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");
            string path1 = Environment.CurrentDirectory + "/OutputFiles/" + "authentication.json";


            System.IO.File.WriteAllText(path1, authentication_json.ToString());
            return RedirectToPage("./AuthenticationPage");
            //How will we redirect last time ????
        }

    }
}