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
        public string[] AlternateName { get; set; }
        
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
            AlternateName = new string[dbstructure.Properties().Count()];
            foreach (var table in dbstructure.Properties())
            {
                Tables[i++] = table.Name.ToString();
            }
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
            //ServerName = Request.Form[nameof(ServerName)];
            //DatabaseName = Request.Form[nameof(DatabaseName)];
            //UserName = Request.Form[nameof(UserName)];
            //Password = Request.Form[nameof(Password)];
            //ConString = Request.Form[nameof(ConString)];

            /*
            string ConnectionString = "Server=" + ServerName + ";Database=" + DatabaseName + ";Uid=" + UserName + ";psw=" + Password + ";";
            ConString = "Connection String :: " + ConnectionString;
            */

            //generate json for db struct and save to file
            try
            {
                string constr = "server=localhost;port=3306;uid=root;pwd=;database=project;charset=utf8;SslMode=none;";
                MySqlConnection con = new MySqlConnection(constr);

                con.Open();
                //Console.WriteLine("connection is " + con.State.ToString());

                DataTable dt = con.GetSchema("Tables");
                JObject dbstructure = new JObject();

                MySqlCommand com = con.CreateCommand();

                foreach (DataRow row in dt.Rows) //for every table
                {
                    string tablename = (string)row[2];
                    JArray columns = new JArray();

                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tablename + "' ORDER BY ORDINAL_POSITION";

                    MySqlDataReader rd = com.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())  //for every column in each table
                        {
                            columns.Add(rd.GetString(0));
                        }
                    }
                    rd.Close();
                    dbstructure.Add(tablename, columns);
                }
                //Console.WriteLine(dbstructure);
                con.Close();

                Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");
                string path = Environment.CurrentDirectory + "/OutputFiles/" + "DB_Structure.json";


                System.IO.File.WriteAllText(path, dbstructure.ToString());
                //return RedirectToPage("./Index");

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message.ToString());

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Error :" + e.Message.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error :" + e.Message.ToString());
            }

            return RedirectToPage("./MappingPage");
        }


    }
}