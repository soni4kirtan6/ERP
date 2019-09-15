using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
