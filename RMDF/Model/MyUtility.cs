using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                JArray inner_mapped_ary = new JArray();
                foreach (JObject inner_obj in inner_ary)
                {
                    JObject inner_mapped_json = new JObject();

                    inner_mapped_json = MyUtility.give_mapped(inner_obj, db_column_mapping[token.Key]);
                    inner_mapped_ary.Add(inner_mapped_json);
                }

                main_input_json[token.Key]["CRUDData"].Replace(inner_mapped_ary);

            }
            JObject main_output_json = new JObject();
            main_output_json = MyUtility.give_mapped(main_input_json, db_table_mapping);

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
            //val_config.Dump();
            foreach (var table in j_inp)
            {
                //val_config[table.Key].Dump();
                //"Table : ".Dump();table.Key.Dump();
                foreach (var data in table.Value["CRUDData"])
                {
                    //"Data : ".Dump();data.Dump();
                    foreach (var col in ((JObject)data).Properties())
                    {
                        //"Col  : ".Dump();
                        //col.Name
                        //val_config[table.Key][col].Dump();
                    }
                    //i++;
                }
            }
            return j_out;
        }
    }
}
