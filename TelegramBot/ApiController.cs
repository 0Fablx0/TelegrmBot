using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
 
namespace TelegramBot
{
    class ApiController
    {
        static HttpClient httpClient = new HttpClient();
        private string _key = "5ae2e3f221c38a28845f05b64677fbc5202d76f89ca368207272ee66";

        public async Task<List<string>> getInterestingPlacesAsync(string sity)
        {
            var geoname = await getGeonameAsync(sity);
            if (geoname.Item1 == null || geoname.Item2 == null)
                return new List<string>() { "This city doesn't exist." };
            return await getPlacesAsync(sity, geoname.Item2, geoname.Item1);
        }

        private async Task<(string, string)> getGeonameAsync(string sity)
        {
            using var response =  await httpClient.GetAsync($"https://api.opentripmap.com/0.1/ru/places/geoname?name={sity}&apikey={_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResp = JObject.Parse(responseBody);
            if (jsonResp["status"].ToString() == "NOT_FOUND")
                return (null,null);
            return (jsonResp["lat"].ToString().Replace(',','.'), jsonResp["lon"].ToString().Replace(',', '.'));
        }

        private async Task<List<string>> getPlacesAsync(string sity, string lon, string lat)
        {
            using var response = await httpClient.GetAsync($"https://api.opentripmap.com/0.1/ru/places/radius?radius=2000&lon={lon}&lat={lat}&rate=3&format=json&limit=20&apikey={_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResp = JArray.Parse(responseBody);
            return JobjToStr(sity,jsonResp);
        }

        private List<string> JobjToStr (string sity, JArray places)
        {
            List<string> retStr = new List<string>();
            sity = char.ToUpper(sity[0]) + sity.Substring(1);
            retStr.Add($"Attractions within a radius of two kilometers from the center {sity}:\n");
            foreach (JObject x in places)
            {
                retStr.Add(x["name"] + "  Coordinates  " + "lon: " + x["point"]["lon"] + "  lat: " + x["point"]["lat"] + "\n");
            }
            return retStr;
        }
    }
}
