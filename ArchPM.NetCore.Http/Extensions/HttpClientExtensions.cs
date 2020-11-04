using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArchPM.NetCore.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace ArchPM.NetCore.Http.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> Request<TResponse>(this HttpClient httpClient, Action<HttpRequestConfiguration> configAction)
        {
            configAction.ThrowExceptionIfNull<ArgumentNullException>($"{nameof(configAction)} must be implemented!");
            var config = new HttpRequestConfiguration();
            configAction(config);

            config.Logger?.LogTrace($"Start sending a request: {config.HttpMethod.Method} {config.RequestUri}");
            var requestMessage = new HttpRequestMessage(config.HttpMethod, config.RequestUri);

            if (config.Data != null)
            {
                config.Logger?.LogTrace("Payload given, start handling serialization...");
                var jsonData = JsonConvert.SerializeObject(config.Data, config.SerializeSettings);
                config.Logger?.LogDebug($"Serialized data: {jsonData}");

                config.Logger?.LogDebug($"Sending HttpRequest: {config.HttpMethod.Method} {config.RequestUri}");
                requestMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            }

            config.Logger?.LogTrace("Sending request...");
            var responseMessage = await httpClient.SendAsync(requestMessage);
            config.Logger?.LogDebug($"Got a response: {responseMessage.StatusCode}");

            if ((int)responseMessage.StatusCode >= config.MinResponseCode && (int)responseMessage.StatusCode <= config.MaxResponseCode)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                var proFileResponses = JsonConvert.DeserializeObject<TResponse>(content, config.DeserializeSettings);

                config.Logger?.LogDebug($"Response content was deserialized to {typeof(TResponse).Name}. Response: {content}");

                return proFileResponses;
            }

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                config.Logger?.LogDebug("The API returned 404 - Not Found. Returns null.");
                return default;
            }

            config.Logger?.LogDebug("The API returned something else than 404 or [200:300] range. Exception will be thrown.");
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

