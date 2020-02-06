using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigPortal.Pages
{
    public class ConfigurationPortal_1Model : PageModel
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConString { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {

            ServerName = Request.Form[nameof(ServerName)];
            DatabaseName = Request.Form[nameof(DatabaseName)];
            UserName = Request.Form[nameof(UserName)];
            Password = Request.Form[nameof(Password)];
            ConString = Request.Form[nameof(ConString)];

            string ConnectionString = "Server=" + ServerName + ";Database=" + DatabaseName + ";Uid=" + UserName + ";psw=" + Password + ";";
            ConString = "Connection String :: " + ConnectionString;

            Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "ConnectionString.txt";

            System.IO.File.WriteAllText(path, ConnectionString);
        }

    }
}