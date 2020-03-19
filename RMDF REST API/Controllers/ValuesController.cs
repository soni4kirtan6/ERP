using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMDF_REST_API.Model;
using Newtonsoft.Json.Linq;

namespace RMDF_REST_API.Controllers
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
            /*string conString = "Server=localhost;Database=project;Uid=root;psw=;";
            MySqlConnection con = new MySqlConnection(conString);
            MySqlCommand cmd;
            con.Open();
            try
            {
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO `person_info`(`person_id`, `first_name`, `middle_name`, `last_name`, `gender`) VALUES (101,\"Ghanshyam\",\"Dharmdev\",\"Pande\",\"Male\")";
                cmd.ExecuteNonQuery();*/

            //Mapping FE json to Db_understandable json
            JObject mapped_json = MyUtility.MappingEngine(value);


            //Validation Engine
            JObject validated_json = MyUtility.ValidationEngine(mapped_json);

            return new string[] { "mapped_json", mapped_json.ToString(), "\nvalidated_json", validated_json.ToString() };
        }
        /* catch (Exception)
         {
             return new string[] { "SQL Error" };
         }
         finally
         {
             con.Close();
         }
*/

        //}



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
