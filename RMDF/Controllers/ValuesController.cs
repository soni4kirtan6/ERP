using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using RMDF.Model;

namespace RMDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value="+id;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<IEnumerable<string>> Post([FromBody] string value)
        {
            JObject main_input_json= JObject.Parse(value);

           
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
            return new string[] { main_output_json.ToString()};
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
    }
}
