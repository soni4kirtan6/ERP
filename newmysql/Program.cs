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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# : Connect to mysql database localhost ");

            JObject map_table_name = new JObject();
            JObject table_column = new JObject();
            JObject val_json = new JObject();

            try
            {
                string constr = "server=localhost;port=3306;uid=root;pwd=;database=sumit;charset=utf8;SslMode=none;" ;
                MySqlConnection con = new MySqlConnection(constr);

                con.Open();
                Console.WriteLine("connection is "+con.State.ToString());

                DataTable dt = con.GetSchema("Tables");
                List<string> tables = new List<string>();

                MySqlCommand com = con.CreateCommand();

                string map_table,map_column;

                foreach (DataRow row in dt.Rows) //for every table
                {
                    JObject columns = new JObject();
                    JObject val_column = new JObject();

                    int num_of_validation;

                    map_table = "";
                    string tablename = (string)row[2];
                    Console.WriteLine("enter the mapping name for table name="+tablename+ "\t");
                    map_table = Console.ReadLine();

                    map_table_name.Add(tablename,map_table);

                    tables.Add(tablename);

                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='"+tablename+"' ORDER BY ORDINAL_POSITION";

                    MySqlDataReader rd = com.ExecuteReader();
                  //  string str = "";

                    if (rd.HasRows)
                    {
                        while (rd.Read())  //for every column in each table
                        {
                          //  str += rd.GetString(0) + "\t";
                            Console.WriteLine("enter the mapping name for column name=" + rd.GetString(0) + "\t");
                            map_column = Console.ReadLine();
                            columns.Add(rd.GetString(0), map_column);

                            Console.WriteLine("how many validation you apply on "+ rd.GetString(0));
                            num_of_validation = Convert.ToInt32(Console.ReadLine());

                            JArray column_validation = new JArray();                        

                            for(int i=1;i<=num_of_validation;i++)
                            {
                                JObject one_val = new JObject();
                                Console.WriteLine("Enter Type key for validation number=" + i);
                                one_val.Add("typeKey", Console.ReadLine());
                                Console.WriteLine("Enter KeyValue for validation number=" + i);
                                one_val.Add("keyvalue", Console.ReadLine());
                                Console.WriteLine("Enter errorOrWarning for validation number=" + i);
                                one_val.Add("errorOrWarning", Console.ReadLine());
                                Console.WriteLine("Enter MsgTextNo for validation number=" + i);
                                one_val.Add("MsgTextNo", Console.ReadLine());

                                column_validation.Add(one_val);
                            }

                            val_column.Add(rd.GetString(0), column_validation);

                        }

                        rd.Close();
                    }
                    //   Console.WriteLine(str);
                    val_json.Add(tablename,val_column);
                    table_column.Add(tablename,columns);
                }


                /* Console.WriteLine("mapping json of table\n"+map_table_name);
                   Console.WriteLine("\nmapping json of columns\n");
                   foreach (var pair in table_column)
                   {
                       Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                   } */

                Console.WriteLine(val_json);

                con.Close();
                Console.WriteLine("connection is " + con.State.ToString());

            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message.ToString());
            }

            Console.WriteLine("press any key ");
            Console.Read();
        }
    }
}
