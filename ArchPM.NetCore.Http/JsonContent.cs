using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ArchPM.NetCore.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(object data, JsonSerializerSettings settings)
            : base(JsonConvert.SerializeObject(data, Formatting.None, settings), Encoding.UTF8, "application/json")
        {
        }

        public static JsonContent From(object data)
        {
            if (data == null)
            {
                return null;
            }

            return new JsonContent(data, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });
        }
    }
}
