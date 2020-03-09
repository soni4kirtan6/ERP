using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Text;

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





// using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
//using System.Data;
//using Newtonsoft.Json.Linq;

//namespace newmysql
//{
//    class Program
//    {
//        //static void Main(string[] args)
//        //{
//        //    Console.WriteLine("C# : Connect to mysql database localhost ");

//        //    JObject map_table_name = new JObject();
//        //    JObject table_column = new JObject();
//        //    JObject val_json = new JObject();

//        //    try
//        //    {
//        //        string constr = "server=localhost;port=3306;uid=root;pwd=;database=sumit;charset=utf8;SslMode=none;" ;
//        //        MySqlConnection con = new MySqlConnection(constr);

//        //        con.Open();
//        //        Console.WriteLine("connection is "+con.State.ToString());

//        //        DataTable dt = con.GetSchema("Tables");
//        //        List<string> tables = new List<string>();

//        //        MySqlCommand com = con.CreateCommand();

//        //        string map_table,map_column;

//        //        foreach (DataRow row in dt.Rows) //for every table
//        //        {
//        //            JObject columns = new JObject();
//        //            JObject val_column = new JObject();

//        //            int num_of_validation;

//        //            map_table = "";
//        //            string tablename = (string)row[2];
//        //            Console.WriteLine("enter the mapping name for table name="+tablename+ "\t");
//        //            map_table = Console.ReadLine();

//        //            map_table_name.Add(tablename,map_table);

//        //            tables.Add(tablename);

//        //            com.CommandType = System.Data.CommandType.Text;
//        //            com.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='"+tablename+"' ORDER BY ORDINAL_POSITION";

//        //            MySqlDataReader rd = com.ExecuteReader();
//        //          //  string str = "";

//        //            if (rd.HasRows)
//        //            {
//        //                while (rd.Read())  //for every column in each table
//        //                {
//        //                  //  str += rd.GetString(0) + "\t";
//        //                    Console.WriteLine("enter the mapping name for column name=" + rd.GetString(0) + "\t");
//        //                    map_column = Console.ReadLine();
//        //                    columns.Add(rd.GetString(0), map_column);

//        //                    Console.WriteLine("how many validation you apply on "+ rd.GetString(0));
//        //                    num_of_validation = Convert.ToInt32(Console.ReadLine());

//        //                    JArray column_validation = new JArray();                        

//        //                    for(int i=1;i<=num_of_validation;i++)
//        //                    {
//        //                        JObject one_val = new JObject();
//        //                        Console.WriteLine("Enter Type key for validation number=" + i);
//        //                        one_val.Add("typeKey", Console.ReadLine());
//        //                        Console.WriteLine("Enter KeyValue for validation number=" + i);
//        //                        one_val.Add("keyvalue", Console.ReadLine());
//        //                        Console.WriteLine("Enter errorOrWarning for validation number=" + i);
//        //                        one_val.Add("errorOrWarning", Console.ReadLine());
//        //                        Console.WriteLine("Enter MsgTextNo for validation number=" + i);
//        //                        one_val.Add("MsgTextNo", Console.ReadLine());

//        //                        column_validation.Add(one_val);
//        //                    }

//        //                    val_column.Add(rd.GetString(0), column_validation);

//        //                }

//        //                rd.Close();
//        //            }
//        //            //   Console.WriteLine(str);
//        //            val_json.Add(tablename,val_column);
//        //            table_column.Add(tablename,columns);
//        //        }


//        //        /* Console.WriteLine("mapping json of table\n"+map_table_name);
//        //           Console.WriteLine("\nmapping json of columns\n");
//        //           foreach (var pair in table_column)
//        //           {
//        //               Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
//        //           } */

//        //        Console.WriteLine(val_json);

//        //        con.Close();
//        //        Console.WriteLine("connection is " + con.State.ToString());

//        //    }
//        //    catch(MySql.Data.MySqlClient.MySqlException ex)
//        //    {
//        //        Console.WriteLine("Error :" + ex.Message.ToString());
//        //    }

//        //    Console.WriteLine("press any key ");
//        //    Console.Read();
//        //}
//        static void Main(string[] args)
//        {
//            Console.WriteLine("C# : Connect to mysql database localhost ");

//            try
//            {
//                string constr = "server=localhost;port=3306;uid=root;pwd=;database=sumit;charset=utf8;SslMode=none;";
//                MySqlConnection con = new MySqlConnection(constr);

//                con.Open();
//                Console.WriteLine("connection is " + con.State.ToString());

//                DataTable dt = con.GetSchema("Tables");
//                JObject dbstructure = new JObject();

//                MySqlCommand com = con.CreateCommand();

//                foreach (DataRow row in dt.Rows) //for every table
//                {
//                    string tablename = (string)row[2];
//                    JArray columns = new JArray();

//                    com.CommandType = System.Data.CommandType.Text;
//                    com.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tablename + "' ORDER BY ORDINAL_POSITION";

//                    MySqlDataReader rd = com.ExecuteReader();

//                    if (rd.HasRows)
//                    {
//                        while (rd.Read())  //for every column in each table
//                        {
//                            columns.Add(rd.GetString(0));
//                        }
//                    }
//                    rd.Close();
//                    dbstructure.Add(tablename, columns);
//                }
//                Console.WriteLine(dbstructure);
//                con.Close();

//            }
//            catch (MySql.Data.MySqlClient.MySqlException ex)
//            {
//                Console.WriteLine("Error :" + ex.Message.ToString());

//            }

//            Console.ReadKey();
//        }
//    }
//}
