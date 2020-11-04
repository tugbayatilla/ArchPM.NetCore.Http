using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArchPM.NetCore.Http.Extensions
{
    //string requestUri, HttpMethod httpMethod, object data, ILogger logger = null
    public class HttpRequestConfiguration
    {
        public HttpRequestConfiguration()
        {
            SetDefaultDeserializeSettings();
            SetDefaultSerializeSettings();
        }


        public string RequestUri { get; set; }
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;
        public object Data { get; set; } = null;
        public ILogger Logger { get; set; } = null;
        public JsonSerializerSettings DeserializeSettings { get; set; }
        public JsonSerializerSettings SerializeSettings { get; set; }
        public int MinResponseCode { get; set; } = 200;
        public int MaxResponseCode { get; set; } = 300;


        public void SetDefaultDeserializeSettings()
        {
            if (DeserializeSettings == null)
            {
                DeserializeSettings = new JsonSerializerSettings();
            }

            DeserializeSettings.Error = (obj, args) =>
            {
                var contextErrors = args.ErrorContext;
                contextErrors.Handled = true;
            };
        }

        internal void SetDefaultSerializeSettings()
        {
            if (SerializeSettings == null)
            {
                SerializeSettings = new JsonSerializerSettings();
            }

            SerializeSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            SerializeSettings.NullValueHandling = NullValueHandling.Ignore;

        }

    }
}
