using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Permissions;
using System.Management;
using System.Configuration.Assemblies;

namespace RMDF_REST_API.Model
{
    public class MyUtility
    {
        public static JObject Give_mapped(JObject j_inp, JToken mapping)
        {
            JObject j_out = new JObject();
            foreach (var prop in j_inp.Properties())//PersonInfo or PersonId
            {
                //for existance of [ PersonId | PersonInfo token in mapping]
                if (mapping[prop.Name] == null)
                {
                    j_out.Add("e_or_w", "e");
                    j_out.Add("code", "101");
                    j_out.Add("msg", prop.Name + " is missing in mapping");
                    j_out.Add("error_entity", prop.Name);

                    return j_out;
                }
                //for checking null/empty value [ PersonId | PersonInfo=="" ]
                else if (mapping[prop.Name].ToString() == "")
                {
                    j_out.Add("e_or_w", "e");
                    j_out.Add("code", "102");
                    j_out.Add("msg", prop.Name + " contains null value in mapping");
                    j_out.Add("error_entity", prop.Name);

                    return j_out;
                }
                                                                                               //change
                j_out.Add(mapping[prop.Name].ToString(), j_inp[prop.Name]);
            }
            return j_out;
        }
        public static JObject MappingEngine(string value)
        {
            JObject error_handler = new JObject();
            //CHACK THE JSON FORMATE
            try
            {
                JObject input = JObject.Parse(value);
                JObject main_input_json =(JObject) input["Operations"];

                string text;

                var fileStream = new FileStream(@"C:\Users\Sumit Patel\source\repos\ERP\ConfigPortal\OutputFiles\table_mapping.json", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                JObject db_table_mapping = JObject.Parse(text);

                string text1;
                var fileStream1 = new FileStream(@"C:\Users\Sumit Patel\source\repos\ERP\ConfigPortal\OutputFiles\col_mapping.json", FileMode.Open, FileAccess.Read);
                using (var streamReader1 = new StreamReader(fileStream1, Encoding.UTF8))
                {
                    text1 = streamReader1.ReadToEnd();
                }
                JObject db_column_mapping = JObject.Parse(text1);

                foreach (var token in main_input_json)
                {
                    // for checking CRUDDATA protocol
                    if (main_input_json[token.Key]["CRUDData"] == null)
                    {

                        error_handler.Add("e_or_w", "e");
                        error_handler.Add("code", "103");
                        error_handler.Add("msg", "Json protocol mis matched");

                        return error_handler;
                    }


                    JArray inner_ary = JArray.Parse(main_input_json[token.Key]["CRUDData"].ToString());
                    //inner_ary.Contains
                    JArray inner_mapped_ary = new JArray();
                    foreach (JObject inner_obj in inner_ary)
                    {
                        JObject inner_mapped_json = new JObject();


                        inner_mapped_json = MyUtility.Give_mapped(inner_obj, db_column_mapping[token.Key]);
                        //check for error
                        if (inner_mapped_json["e_or_w"] != null)
                        {
                            return inner_mapped_json;
                        }
                        inner_mapped_ary.Add(inner_mapped_json);
                    }

                    main_input_json[token.Key]["CRUDData"].Replace(inner_mapped_ary);

                }

                JObject main_output_json = Give_mapped(main_input_json, db_table_mapping);
                //check for error not needed here

                 return main_output_json;


            }
            catch (Exception ex)
            {
                error_handler.Add("e_or_w", "e");
                error_handler.Add("code", "102");
                error_handler.Add("msg", ex.Message + " json formate error");

                return error_handler;

            }
        }

        public static JObject ValidationEngine(JObject j_inp)
        {
            
            JObject j_out = new JObject();
            string text;

            var fileStream = new FileStream(@"C:\Users\Sumit Patel\source\repos\ERP\ConfigPortal\OutputFiles\validationConfig.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            { text = streamReader.ReadToEnd(); }
            JObject val_config = JObject.Parse(text);
            //val_config.Dump();
            //"Validation Config".Dump();
            //val_config.Dump();
            JObject validation_out = new JObject();
            foreach (var table in j_inp)
            {

                //val_config[table.Key].Dump();
                //"Table : ".Dump();table.Key.Dump();
                JArray valid_table = new JArray();
                foreach (var data in table.Value["CRUDData"])
                {
                    //"Data : ".Dump(); data.Dump();
                    JObject valid_col = new JObject();
                    foreach (var col in ((JObject)data).Properties())
                    {
                        //"\nCol  : ".Dump();
                        //("\nCol  : "+col.Name + " : "+col.Value).Dump();
                        //"Col".Dump();
                        //col.Dump();
                        int e_count = 0, w_count = 0;
                        JArray msg_code = new JArray();
                        JObject val_out = new JObject();

                        try
                        {
                            JArray validation_ary_4_col = JArray.Parse(val_config[table.Key][col.Name].ToString());
                            //"check".Dump();

                            foreach (var validation in validation_ary_4_col)
                            {
                                //validation.Dump();
                                string val_name = validation["typeKey"].ToString();
                                switch (val_name)
                                {
                                    case "canBeEmpty":
                                        bool null_cond = col.Value != null && col.Value.ToString() != "";
                                        bool cond_2 = validation["keyValue"].ToString().Equals("true");
                                        if (cond_2 || null_cond)
                                        {
                                            //(val_name+" : Pass").Dump();
                                            val_out.Add("Value", col.Value);
                                            msg_code.Add("2001");
                                        }
                                        else
                                        {
                                            if (validation["errorOrWarning"].ToString() == "e")
                                                e_count++;
                                            else
                                                w_count++;
                                            //	(val_name+" : Fail").Dump();
                                            //	val_out.Add("msgCode",validation["MsgTextNo"].ToString());
                                            msg_code.Add(validation["MsgTextNo"].ToString());
                                        }
                                        break;
                                    case "minLength":
                                        if (col.Value.ToString().Length >= Convert.ToInt32(validation["keyValue"].ToString()))
                                        {
                                            //(val_name+" : Pass").Dump();
                                            msg_code.Add("2002");
                                        }
                                        else
                                        {
                                            if (validation["errorOrWarning"].ToString() == "e")
                                                e_count++;
                                            else
                                                w_count++;
                                            //(val_name+" : Fail").Dump();
                                            msg_code.Add(validation["MsgTextNo"].ToString());
                                        }
                                        break;
                                    case "maxLength":
                                        if (col.Value.ToString().Length <= Convert.ToInt32(validation["keyValue"].ToString()))
                                        {
                                            //(val_name+" : Pass").Dump();
                                            msg_code.Add("2003");
                                        }
                                        else
                                        {
                                            if (validation["errorOrWarning"].ToString() == "e")
                                                e_count++;
                                            else
                                                w_count++;
                                            //(val_name+" : Fail").Dump();
                                            msg_code.Add(validation["MsgTextNo"].ToString());
                                        }
                                        break;
                                    case "RegEx":


                                        string text1;
                                        var fileStream1 = new FileStream(@"JsonFiles\defaultRegEx.json", FileMode.Open, FileAccess.Read);
                                        using (var streamReader1 = new StreamReader(fileStream1, Encoding.UTF8))
                                        { text1 = streamReader1.ReadToEnd(); }
                                        JObject default_reg_ex = JObject.Parse(text1);

                                        Regex rgx = new Regex(default_reg_ex[validation["keyValue"].ToString()].ToString());
                                        //rgx.Dump();
                                        string input = col.Value.ToString();
                                        if (rgx.IsMatch(input))
                                        {
                                            //(val_name+" : Pass").Dump();
                                            msg_code.Add("2004");
                                        }
                                        else
                                        {
                                            if (validation["errorOrWarning"].ToString() == "e")
                                                e_count++;
                                            else
                                                w_count++;
                                            //(val_name+" : Fail").Dump();
                                            msg_code.Add(validation["MsgTextNo"].ToString());
                                        }
                                        break;

                                    case "staticList":
                                        JArray static_list = JArray.Parse(validation["keyValue"].ToString());
                                        // static_list.Dump();
                                        string[] items = static_list.Select(jv => (string)jv).ToArray();
                                        if (items.Contains(col.Value.ToString()))
                                        {
                                            //(val_name+" : Pass").Dump();
                                            msg_code.Add("2005");
                                        }
                                        else
                                        {
                                            if (validation["errorOrWarning"].ToString() == "e")
                                                e_count++;
                                            else
                                                w_count++;
                                            //(val_name+" : Fail").Dump();
                                            msg_code.Add(validation["MsgTextNo"].ToString());
                                        }
                                        break;
                                    default:
                                        //"Validation not Found".Dump();
                                        break;
                                }
                            }
                            if (e_count == 0 && w_count == 0)
                            {
                                val_out.Add("status", "success");
                                val_out.Add("w_count", w_count.ToString());
                                val_out.Add("e_count", e_count.ToString());
                            }
                            else
                            {
                                if (e_count == 0 && w_count != 0)
                                {
                                    val_out.Add("status", "warning");
                                    val_out.Add("w_count", w_count.ToString());
                                    val_out.Add("e_count", e_count.ToString());
                                }
                                else
                                {
                                    val_out.Add("status", "error");
                                    val_out.Add("w_count", w_count.ToString());
                                    val_out.Add("e_count", e_count.ToString());
                                }
                            }
                            val_out.Add("message_code", msg_code);
                            //	val_out.Dump();
                        }
                        catch (NullReferenceException)
                        {
                            //e.Dump();
                            //"No Validation Required !!";
                        }
                        //	col.Dump();
                        valid_col.Add(col.Name.ToString(), val_out);
                    }
                    //i++;
                    //	valid_col.Dump();
                    valid_table.Add(valid_col);
                }
                //valid_table.Dump();
                //	table.ToString().Dump();
                validation_out.Add(table.Key.ToString(), valid_table);
            }
            //validation_out.Dump();
            j_out = validation_out;
            return j_out;
        }


        public static JObject Authentication_query(JObject inp_json)
        {
            string text;

            JObject Auth_out = new JObject();
            JArray output = new JArray();


            var fileStream = new FileStream(@"C:\Users\Sumit Patel\source\repos\ERP\ConfigPortal\OutputFiles\Authentication.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            { text = streamReader.ReadToEnd(); }
            JObject Auth_config = JObject.Parse(text);

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
                int count = 1;
                //		table.Value.Dump();
                //		table.Value["CRUDType"].ToString().Dump();

                CRUD = user_role[table.Key]["CRUD"].ToString();

                JObject jout = new JObject();
                JArray write_cols = new JArray();
                JArray read_cols = new JArray();

                write_cols = (JArray)(user_role[table.Key]["!WriteColumns"]);
                read_cols = (JArray)(user_role[table.Key]["!ReadColumns"]);
                //      write_cols.Dump();


                switch (table.Value["CRUD"].ToString())
                {
                    case "C":
                        if (CRUD[0].ToString() == "C")
                        {
                            foreach (JObject columns in table.Value["CRUDData"])
                            {
                                JObject each_jobject = new JObject();
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

                                        string constr = "server=localhost;port=3306;uid=root;pwd=;database=testcase;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);



                                        MySqlCommand com = con.CreateCommand();

                                        com = new MySqlCommand(query, con);

                                        con.Open();
                                        com.ExecuteNonQuery();

                                        //  Console.WriteLine("success");
                                        each_jobject.Add(""+count,"New row created");
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
                                jout.Add(table.Key + " " + count++, each_jobject);
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
                                JObject each_jobject = new JObject();
                                JArray col_names = new JArray();
                                JArray col_values = new JArray();
                                foreach (var col in columns)
                                {
                                    //col.Key.Dump();
                                    col_names.Add(col.Key);
                                    col_values.Add(col.Value);
                                    foreach (var i in read_cols)
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
                                        query = query + col_names[i] + " from " + table.Key.ToString() + " where " + col_names[0] + "="+col_values[0];

                                        Console.WriteLine(query);

                                        string constr = "server=localhost;port=3306;uid=root;pwd=;database=testcase;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);

                                        DataTable dt = new DataTable();                                                                     //for read
                                        MySqlDataAdapter adapt;
                                        adapt = new MySqlDataAdapter(query, con);
                                        adapt.Fill(dt);

                                       // JObject jout = new JObject();
                                        JArray columns_ = new JArray();
                                        JArray rows = new JArray();
                                        string ColumnName;
                                        foreach (DataColumn column in dt.Columns)
                                        {

                                            ColumnName = column.ColumnName;
                                            columns_.Add(ColumnName);

                                        }
                                        each_jobject.Add("Columns", columns_);
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
                                        each_jobject.Add("Rows", rows);
                                        //   Console.WriteLine(jout);

                                     //   output.Add(jout);


                                     //   Console.WriteLine("success");
                                     //   con.Close();

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
                                jout.Add(table.Key+" "+count++,each_jobject);
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
                                JObject each_jobject = new JObject();
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

                                        string constr = "server=localhost;port=3306;uid=root;pwd=;database=testcase;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);



                                        MySqlCommand com = con.CreateCommand();

                                        com = new MySqlCommand(query, con);
                                        //for update

                                        con.Open();
                                        com.ExecuteNonQuery();

                                        each_jobject.Add("" + count, "Row Updated");
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
                                jout.Add(table.Key + " " + count++, each_jobject);
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
                                JObject each_jobject = new JObject();
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

                                        string constr = "server=localhost;port=3306;uid=root;pwd=;database=testcase;charset=utf8;SslMode=none;";
                                        MySqlConnection con = new MySqlConnection(constr);



                                        MySqlCommand com = con.CreateCommand();

                                        com = new MySqlCommand(query, con);
                                        //for update

                                        con.Open();
                                        com.ExecuteNonQuery();

                                        each_jobject.Add("" + count, "Row deleted");
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
                                jout.Add(table.Key + " " + count++, each_jobject);
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
                output.Add(jout);
                //Auth_out.Add("output",output); 
            }
            Auth_out.Add("output", output);

            return Auth_out;

        }

    }

}
