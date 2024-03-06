using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValyutakursBot
{
    public static class Convertion
    {
        public static List<object> ConnectWithJson()
        {
            HttpClient client = new HttpClient();

            var reques = new HttpRequestMessage(HttpMethod.Get, "https://www.nbu.uz/exchange-rates/json/");

            var respone = client.SendAsync(reques).Result;

            var body = respone.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<object>>(body);

        }
        public static string ConvertionMoney(List<object> info, string CountryCode)
        {
            List<object> list = info;
            for (int i = 0; i < list.Count; i++)
            {
                JObject temp = JObject.Parse(JsonConvert.SerializeObject(list[i]));
                if (temp["code"].ToString().Equals(CountryCode))
                {
                    return temp["nbu_cell_price"].ToString();
                }
            }
            return String.Empty;
        }

    }
}

