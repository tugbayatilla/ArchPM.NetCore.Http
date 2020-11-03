using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArchPM.NetCore.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> Request<TResponse>(this HttpClient httpClient, string requestUri, HttpMethod httpMethod, object data, ILogger logger = null)
        {
            logger?.LogTrace($"Start sending a request: {httpMethod.Method} {requestUri}");
            var requestMessage = new HttpRequestMessage(httpMethod, requestUri);

            if (data != null)
            {
                logger?.LogTrace("Payload given, start handling serialization...");
                var jsonPayload = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                });
                logger?.LogDebug($"Serialized data: {jsonPayload}");

                logger?.LogDebug($"Sending HttpRequest: {httpMethod.Method} {requestUri}");
                requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            }

            logger?.LogTrace("Sending request...");
            var responseMessage = await httpClient.SendAsync(requestMessage);
            logger?.LogDebug($"Got a response: {responseMessage.StatusCode}");

            if ((int)responseMessage.StatusCode >= 200 && (int)responseMessage.StatusCode <= 300)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                var proFileResponses = JsonConvert.DeserializeObject<TResponse>(content);

                logger?.LogDebug($"Response content was deserialized to {typeof(TResponse).Name}. Response: {content}");

                return proFileResponses;
            }

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogDebug("The API returned 404 - Not Found. Returns null.");
                return default;
            }

            logger?.LogDebug("The API returned something else than 404 or [200:300] range. Exception will be thrown.");
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new Exception(
                $"Request resulted in error. StatusCode: {responseMessage.StatusCode}. Response: {errorContent}"
            );
        }

        public static HttpClient CreateMockHttpClient<T>(T resultObject)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new JsonContent(
                            resultObject,
                            new JsonSerializerSettings())
                    }
                );
            var httpClientMocked = new HttpClient(mockMessageHandler.Object);

            return httpClientMocked;
        }
    }
}

