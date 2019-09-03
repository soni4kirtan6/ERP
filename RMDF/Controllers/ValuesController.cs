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
            return "value";
        }

        // POST api/values
        [HttpPost]
        public ActionResult<IEnumerable<string>> Post([FromBody] string value)
        {
            JObject input= JObject.Parse(value);

            string[] r=new string[10];
            int i = 0;
            foreach (var item in input)
            {
                r[i++]=(item.Key +"    :    "+ item.Value+",");
            }

            string text;
            var fileStream = new FileStream(@"C:\Users\user\Source\Repos\ERP\RMDF\JsonFiles\db_table_mapping.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
                //JObject db_table_mapping = JObject.Parse(text);

            }
            JObject db_table_mapping = JObject.Parse(text);
            return new string[] { db_table_mapping.ToString() };
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
