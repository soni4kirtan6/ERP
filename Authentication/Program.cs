using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Security.Permissions;
using System.Management;
using System.Configuration.Assemblies;

namespace Authentication
{
    class Program
    {
        static void Main(string[] args)
        {
            string text, text1;

            var fileStream = new FileStream(@"C:\Users\Sumit Patel\source\repos\ERP\RMDF\JsonFiles\Authentication1.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            { text = streamReader.ReadToEnd(); }
            JObject Auth_config = JObject.Parse(text);

            var fileStream1 = new FileStream(@"C:\Users\Sumit Patel\source\repos\ERP\RMDF\JsonFiles\input_from_FE1.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream1, Encoding.UTF8))
            { text1 = streamReader.ReadToEnd(); }
            JObject inp_json = JObject.Parse(text1);

            JObject user_role = new JObject(); 

            int flag = 0;

            //get user role from authentication json file
            foreach (var role in Auth_config)
            {
                if (inp_json["User Role"].ToString() == role.Key.ToString())
                {
                    user_role = (JObject)(role.Value);
                    flag = 1;
                }
            }
            if (flag == 0)
            {
                Console.WriteLine("User Role Doesn't exist");
            }

            string CRUD;

            foreach (var table in (JObject)inp_json["Operations"])
            {
                int temp = 0;
                //		table.Value.Dump();
                //		table.Value["CRUDType"].ToString().Dump();

                CRUD = user_role[table.Key]["CRUD"].ToString();
                JArray write_cols = new JArray();
                JArray read_cols = new JArray();

                write_cols = (JArray)(user_role[table.Key]["!WriteColumns"]);
                read_cols = (JArray)(user_role[table.Key]["!ReadColumns"]);
                //      write_cols.Dump();


                switch (table.Value["CRUDType"].ToString())
                {
                    case "C":
                        if (CRUD[0].ToString() == "C")
                        {
                            foreach (JObject columns in table.Value["CRUDData"])
                            {
                                JArray col_names = new JArray();
                                JArray col_values = new JArray();
                                foreach (var col in columns)
                                {
                                    //col.Key.Dump();
                                    col_names.Add(col.Key);
                                    col_values.Add(col.Value);
                                    foreach (var i in write_cols)
                                    {
                                        if (i.ToString() == col.Key)
                                        {
                                            //	i.Dump();
                                            //	col.Key.Dump();
                                            temp = 1;
                                        }
                                    }
                                }

                                if (temp == 0)
                                {

                                    try
                                    {

                                        string query1 = "";
                                        string query = "insert into " + table.Key.ToString() + "(";
                                        int i;
                                        for (i = 0; i < col_names.Count - 1; i++)
                                        {
                                            query = query + col_names[i] + ",";
                                            if (i == 0)
                                            {
                                                query1 = query1 + col_values[i] + ",";
                                            }
                                            else
                                            {
                                                query1 = query1 + "\"" + col_values[i] + "\"" + ",";
                                            }
                                        }
                                        query = query + col_names[i] + ") values(";
                                        query1 = query1 + "\"" + col_values[i] + "\")";
                                        query = query + query1;

                                        Console.WriteLine(query);

                                        string constr = "server=localhost;port=3306;uid=root;pwd=;database=test;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);



                                        MySqlCommand com = con.CreateCommand();

                                        com = new MySqlCommand(query, con);            

                                        con.Open();
                                        com.ExecuteNonQuery();

                                        Console.WriteLine("success");
                                        con.Close();

                                    }
                                    catch (MySql.Data.MySqlClient.MySqlException ex)
                                    {
                                        Console.WriteLine("Error :" + ex.Message.ToString());
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("failure");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("permission is not allowed");
                        }
                        break;

                    case "R":
                        if (CRUD[1].ToString() == "R")
                        {
                                foreach (JObject columns in table.Value["CRUDData"])
                                {
                                    JArray col_names = new JArray();
                                    JArray col_values = new JArray();
                                    foreach (var col in columns)
                                    {
                                        //col.Key.Dump();
                                        col_names.Add(col.Key);
                                        col_values.Add(col.Value);
                                        foreach (var i in write_cols)
                                        {
                                            if (i.ToString() == col.Key)
                                            {
                                                //	i.Dump();
                                                //	col.Key.Dump();
                                                temp = 1;
                                            }
                                        }
                                    }
                                    if (temp == 0)
                                {

                                    try
                                    {
                                        string query = "select ";
                                        int i;
                                        for (i = 1; i < col_names.Count - 1; i++)
                                        {
                                            query = query + col_names[i] + ",";
                                        }
                                        query = query + col_names[i] + " from " + table.Key.ToString() + " where " + col_names[0] + "=" + col_values[0];

                                        Console.WriteLine(query);

                                        string constr = "server=localhost;port=3306;uid=root;pwd=;database=test;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);

                                        DataTable dt = new DataTable();                                                                     //for read
                                        MySqlDataAdapter adapt;
                                        adapt = new MySqlDataAdapter(query, con);
                                        adapt.Fill(dt);

                                        JObject jout = new JObject();
                                        JArray columns_ = new JArray();
                                        JArray rows = new JArray();
                                        string ColumnName;
                                        foreach (DataColumn column in dt.Columns)
                                        {

                                            ColumnName = column.ColumnName;
                                            columns_.Add(ColumnName);

                                        }
                                        jout.Add("Columns", columns_);
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            JArray values = new JArray();
                                            string ColumnData;
                                            foreach (DataColumn column in dt.Columns)
                                            {
                                                ColumnData = row[column].ToString();
                                                values.Add(ColumnData);
                                                //  Console.WriteLine(ColumnName + " " + ColumnData);
                                            }
                                            rows.Add(values);
                                        }
                                        jout.Add("Rows", rows);
                                        Console.WriteLine(jout); 


                                        Console.WriteLine("success");
                                        con.Close();                                

                                    }
                                    catch (MySql.Data.MySqlClient.MySqlException ex)
                                    {
                                        Console.WriteLine("Error :" + ex.Message.ToString());
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("failure");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("permission is not allowed");
                        }
                        break;
                    case "U":
                        if (CRUD[2].ToString() == "U")
                        {
                            foreach (JObject columns in table.Value["CRUDData"])
                            {
                                JArray col_names = new JArray();
                                JArray col_values = new JArray();
                                foreach (var col in columns)
                                {
                                    //col.Key.Dump();
                                    col_names.Add(col.Key);
                                    col_values.Add(col.Value);
                                    foreach (var i in write_cols)
                                    {
                                        if (i.ToString() == col.Key)
                                        {
                                            //	i.Dump();
                                            //	col.Key.Dump();
                                            temp = 1;
                                        }
                                    }
                                }

                                if (temp == 0)
                                {  

                                    try
                                    { 


                                    string query = "update " + table.Key.ToString() + " set";
                                    int i;
                                    for (i = 1; i < col_names.Count - 1; i++)
                                    {
                                        query = query + " " + col_names[i] + "=" + "\"" + col_values[i] + "\"" + ",";
                                    }
                                    query = query + " " + col_names[i] + "=" + "\"" + col_values[i] + "\"" + " where " + col_names[0] + "=" + col_values[0];

                                    Console.WriteLine(query);

                                      string constr = "server=localhost;port=3306;uid=root;pwd=;database=test;charset=utf8;SslMode=none;";
                                          MySqlConnection con = new MySqlConnection(constr);

                                        

                                        MySqlCommand com = con.CreateCommand();
                                          
                                          com = new MySqlCommand(query,con);
                                        //for update

                                        con.Open();
                                        com.ExecuteNonQuery();

                                          Console.WriteLine("success");
                                          con.Close();                   

                                    }
                                    catch (MySql.Data.MySqlClient.MySqlException ex)
                                    {
                                          Console.WriteLine("Error :" + ex.Message.ToString());
                                    } 

                                }
                                else
                                {
                                    Console.WriteLine("failure");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("permission is not allowed");
                        }
                        break;
                    case "D":
                        if (CRUD[3].ToString() == "D")
                        {
                            foreach (JObject columns in table.Value["CRUDData"])
                            {
                                JArray col_names = new JArray();
                                JArray col_values = new JArray();
                                foreach (var col in columns)
                                {
                                    //col.Key.Dump();
                                    col_names.Add(col.Key);
                                    col_values.Add(col.Value);
                                    foreach (var i in write_cols)
                                    {
                                        if (i.ToString() == col.Key)
                                        {
                                            //	i.Dump();
                                            //	col.Key.Dump();
                                            temp = 1;
                                        }
                                    }
                                }


                                if (temp == 0)
                                {

                                    try
                                    {


                                        string query = "delete from " + table.Key.ToString();
                                                                          
                                        query = query + " where " + col_names[0] + "=" + col_values[0];

                                        Console.WriteLine(query);

                                       string constr = "server=localhost;port=3306;uid=root;pwd=;database=test;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);



                                        MySqlCommand com = con.CreateCommand();

                                        com = new MySqlCommand(query, con);
                                        //for update

                                        con.Open();
                                        com.ExecuteNonQuery();

                                        Console.WriteLine("success");
                                        con.Close();   

                                    }
                                    catch (MySql.Data.MySqlClient.MySqlException ex)
                                    {
                                        Console.WriteLine("Error :" + ex.Message.ToString());
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("failure");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("permission is not allowed");
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
