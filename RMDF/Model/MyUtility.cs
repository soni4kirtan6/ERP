using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RMDF.Model
{
    public class MyUtility
    {
        public static JObject give_mapped(JObject j_inp, JToken mapping)
        {
            JObject j_out = new JObject();
            foreach (var prop in j_inp.Properties())//PersonInfo
            {
                j_out.Add(mapping[prop.Name].ToString(), j_inp[prop.Name]);
            }
            return j_out;
        }
        public static JObject MappingEngine(string value)
        {
            JObject main_input_json = JObject.Parse(value);


            string text;
            var fileStream = new FileStream(@"JsonFiles\db_table_mapping.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            JObject db_table_mapping = JObject.Parse(text);

            string text1;
            var fileStream1 = new FileStream(@"JsonFiles\tb_column_mapping.json", FileMode.Open, FileAccess.Read);
            using (var streamReader1 = new StreamReader(fileStream1, Encoding.UTF8))
            {
                text1 = streamReader1.ReadToEnd();
            }
            JObject db_column_mapping = JObject.Parse(text1);



            foreach (var token in main_input_json)
            {
                JArray inner_ary = JArray.Parse(main_input_json[token.Key]["CRUDData"].ToString());
                //inner_ary.Contains
                JArray inner_mapped_ary = new JArray();
                foreach (JObject inner_obj in inner_ary)
                {
                    JObject inner_mapped_json = new JObject();

                    inner_mapped_json = MyUtility.give_mapped(inner_obj, db_column_mapping[token.Key]);
                    inner_mapped_ary.Add(inner_mapped_json);
                }

                main_input_json[token.Key]["CRUDData"].Replace(inner_mapped_ary);

            }

            JObject main_output_json = give_mapped(main_input_json, db_table_mapping);

            return main_output_json;
        }

        public static JObject ValidationEngine(JObject j_inp)
        {

            JObject j_out = new JObject();
            string text;
            var fileStream = new FileStream(@"C:\Users\user\Source\Repos\ERP\RMDF\JsonFiles\validationConfig.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            { text = streamReader.ReadToEnd(); }
            JObject val_config = JObject.Parse(text);
            //val_config.////Dump();
            foreach (var table in j_inp)
            {
                //val_config[table.Key].//Dump();
                //"Table : ".//Dump();table.Key.//Dump();
                foreach (var data in table.Value["CRUDData"])
                {
                    //"Data : ".//Dump();data.//Dump();
                    foreach (var col in ((JObject)data).Properties())
                    {
                        //"\nCol  : ".//Dump();
                        //("\nCol  : "+col.Name + " : "+col.Value).//Dump();
                        //Dump(col);

                        try
                        {
                            JArray validation_ary_4_col = JArray.Parse(val_config[table.Key][col.Name].ToString());
                            //"check".//Dump();
                            foreach (var validation in validation_ary_4_col)
                            {
                                //validation.//Dump();
                                string val_name = validation["typeKey"].ToString();
                                switch (val_name)
                                {
                                    case "canBeEmpty":
                                        bool null_cond = col.Value != null && col.Value.ToString() != "";
                                        bool cond_2 = validation["keyValue"].ToString().Equals("true");
                                        if (cond_2 || null_cond)
                                        {
                                            //Dump((val_name + " : Pass"));
                                        }
                                        else
                                        {
                                            //Dump((val_name + " : Fail"));
                                        }

                                        break;
                                    case "minLength":
                                        if (col.Value.ToString().Length >= Convert.ToInt32(validation["keyValue"].ToString()))
                                        {
                                            //Dump((val_name + " : Pass"));
                                        }
                                        else
                                        {
                                            //Dump((val_name + " : Fail"));
                                        }
                                        break;
                                    case "maxLength":
                                        if (col.Value.ToString().Length <= Convert.ToInt32(validation["keyValue"].ToString()))
                                        {
                                            //Dump((val_name + " : Pass"));
                                        }
                                        else
                                        {
                                            //Dump((val_name + " : Fail"));
                                        }
                                        break;
                                    case "RegEx":


                                        string text1;
                                        var fileStream1 = new FileStream(@"C:\Users\user\Source\Repos\ERP\RMDF\JsonFiles\defaultRegEx.json", FileMode.Open, FileAccess.Read);
                                        using (var streamReader1 = new StreamReader(fileStream1, Encoding.UTF8))
                                        { text1 = streamReader1.ReadToEnd(); }
                                        JObject default_reg_ex = JObject.Parse(text1);

                                        Regex rgx = new Regex(default_reg_ex[validation["keyValue"].ToString()].ToString());
                                        //rgx.//Dump();
                                        string input = col.Value.ToString();
                                        if (rgx.IsMatch(input))
                                        {
                                            //Dump((val_name + " : Pass"));
                                        }
                                        else
                                        {
                                        }
                                        //Dump((val_name + " : Fail"));
                                        break;
                                    case "staticList":
                                        JArray static_list = JArray.Parse(validation["keyValue"].ToString());
                                        //Dump(static_list);
                                        string[] items = static_list.Select(jv => (string)jv).ToArray();
                                        if (items.Contains(col.Value.ToString()))
                                        {
                                            //Dump((val_name + " : Pass"));
                                        }
                                        else {
                                        }
                                            //Dump((val_name + " : Fail"));
                                        break;
                                    default:
                                        //Dump("Validation not Fond");
                                        break;
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            //Dump(e);
                            //Dump("No Validation Required !!");
                        }

                    }
                    //i++;
                }
            }
            return j_out;
        }

        public static T Dump<T>(T o)
        {
            Console.WriteLine(o.ToString());
            return o;
        }
    }
    
}
