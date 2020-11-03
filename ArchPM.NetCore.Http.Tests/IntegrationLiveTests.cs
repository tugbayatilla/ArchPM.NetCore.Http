using System.Net.Http;
using System.Threading.Tasks;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;
using FluentAssertions;
using Xunit;

namespace ArchPM.NetCore.Http.Tests
{
    public class IntegrationLiveTests
    {
        
        [Fact]
        public async Task PostMessage()
        {
            var httpClient = new HttpClient();
            IMicrosoftTeamsLogicAppPostMessageClient client = new DefaultMicrosoftTeamsLogicAppPostMessageClient(httpClient, new DefaultMicrosoftTeamsLogicAppClientSettings()
            {
                Active = true,
                EndpointUrl = ""
            });


            var response = await client.SendMessage(
                new DefaultMicrosoftTeamsPostMessage()
                {
                    Subject = "Test From FW",
                    Message = "Lorem Ipsum"
                }
            );

            response.Should().NotBeNull();
            response.MessageId.Should().NotBeNull();

        }

        [Fact]
        public async Task ReplyMessage()
        {
            var httpClient = new HttpClient();
            var postClient = new DefaultMicrosoftTeamsLogicAppPostMessageClient(httpClient, new DefaultMicrosoftTeamsLogicAppClientSettings()
            {
                Active = true,
                EndpointUrl = ""
            });

            var postResponse = await postClient.SendMessage(
                new DefaultMicrosoftTeamsPostMessage()
                {
                    Subject = "Test From FW",
                    Message = "Lorem Ipsum"
                }
            );

            var replyClient = new DefaultMicrosoftTeamsLogicAppReplyMessageClient(httpClient, new DefaultMicrosoftTeamsLogicAppClientSettings()
            {
                Active = true,
                EndpointUrl = ""
            });

            var response = await replyClient.SendMessage(
                new DefaultMicrosoftTeamsReplyMessage()
                {
                    MessageId = postResponse.MessageId,
                    Message = "Lorem Ipsum - reply"
                }
            );

            response.Should().NotBeNull();
            response.MessageId.Should().NotBeNull();

        }





    }
}
