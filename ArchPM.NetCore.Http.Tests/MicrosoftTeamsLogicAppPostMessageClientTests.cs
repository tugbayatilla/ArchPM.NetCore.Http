using System;
using System.Net.Http;
using System.Threading.Tasks;
using ArchPM.NetCore.Builders;
using ArchPM.NetCore.Http.Extensions;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients.Settings;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;
using FluentAssertions;
using Xunit;

namespace ArchPM.NetCore.Http.Tests
{
    public class MicrosoftTeamsLogicAppPostMessageClientTests
    {
        private DefaultMicrosoftTeamsLogicAppPostMessageClient CreateClient(DefaultMicrosoftTeamsLogicAppPostMessageClientSettings settings = null, HttpClient httpClient = null)
        {
            if (settings == null)
            {
                settings = new DefaultMicrosoftTeamsLogicAppPostMessageClientSettings()
                {
                    Active = true,
                    EndpointUrl = "https://loremipsum.com/api/v1/postmessage"
                };
            }

            if (httpClient == null)
            {
                httpClient = HttpClientExtensions.CreateMockHttpClient(
                    new DefaultMicrosoftTeamsPostMessageResponse()
                    {
                        MessageId = "M123456789"
                    }
                );
            }

            var notifier = new DefaultMicrosoftTeamsLogicAppPostMessageClient(httpClient, settings);

            return notifier;
        }

        [Fact]
        public async Task SendMessage_should_return_empty_response_when_settings_is_not_active()
        {

            var client = CreateClient(new DefaultMicrosoftTeamsLogicAppPostMessageClientSettings()
            {
                Active = false
            });

            var response = await client.SendMessage(null);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendMessage_should_throw_argument_null_exception_when_settings_active_and_argument_is_null()
        {

            var client = CreateClient();

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => client.SendMessage(null)
            );
        }

        [Fact]
        public async Task SendMessage_should_throw_exception_when_http_response_is_null()
        {
            var httpClient = HttpClientExtensions.CreateMockHttpClient<object>(null);

            var client = CreateClient(httpClient:httpClient);

            await Assert.ThrowsAsync<Exception>(
                () => client.SendMessage(SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>())
            );
        }

        [Fact]
        public async Task SendMessage_should_throw_exception_when_http_response_messageid_is_null()
        {
            var httpClient = HttpClientExtensions.CreateMockHttpClient(new DefaultMicrosoftTeamsPostMessageResponse()
            {
                MessageId = null
            });

            var client = CreateClient(httpClient: httpClient);

            await Assert.ThrowsAsync<Exception>(
                () => client.SendMessage(SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>())
            );
        }



    }
}
