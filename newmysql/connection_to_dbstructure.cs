using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json.Linq;

namespace newmysql
{
    class connection_to_dbstructure
    {

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("C# : Connect to mysql database localhost ");

        //    try
        //    {
        //        string constr = "server=localhost;port=3306;uid=root;pwd=;database=sumit;charset=utf8;SslMode=none;";
        //        MySqlConnection con = new MySqlConnection(constr);

        //        con.Open();
        //        Console.WriteLine("connection is " + con.State.ToString());

        //        DataTable dt = con.GetSchema("Tables");
        //        JObject dbstructure = new JObject();

        //        MySqlCommand com = con.CreateCommand();

        //        foreach (DataRow row in dt.Rows) //for every table
        //        {
        //            string tablename = (string)row[2];
        //            JArray columns = new JArray();

        //            com.CommandType = System.Data.CommandType.Text;
        //            com.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tablename + "' ORDER BY ORDINAL_POSITION";

        //            MySqlDataReader rd = com.ExecuteReader();

        //            if (rd.HasRows)
        //            {    
        //                while (rd.Read())  //for every column in each table
        //                { 
        //                    columns.Add(rd.GetString(0));
        //                }
        //            }
        //            dbstructure.Add(tablename, columns);
        //        }
        //        Console.WriteLine(dbstructure);
        //    }
        //    catch (MySql.Data.MySqlClient.MySqlException ex)
        //    {
        //        Console.WriteLine("Error :" + ex.Message.ToString());
        //    }
        //}
    }
}
