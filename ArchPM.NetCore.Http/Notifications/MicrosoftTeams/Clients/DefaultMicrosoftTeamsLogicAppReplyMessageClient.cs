using System;
using System.Net.Http;
using System.Threading.Tasks;
using ArchPM.NetCore.Extensions;
using ArchPM.NetCore.Http.Extensions;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;

namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public class DefaultMicrosoftTeamsLogicAppReplyMessageClient : IMicrosoftTeamsLogicAppReplyMessageClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMicrosoftTeamsLogicAppReplyMessageClientSettings _settings;

        public DefaultMicrosoftTeamsLogicAppReplyMessageClient(HttpClient httpClient, IMicrosoftTeamsLogicAppReplyMessageClientSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<IMicrosoftTeamsReplyMessageResponse> SendMessage(IMicrosoftTeamsReplyMessage message)
        {
            if (!_settings.Active)
            {
                return new DefaultMicrosoftTeamsReplyMessageResponse();
            }

            message.ThrowExceptionIfNull<ArgumentNullException>();
            message.ThrowExceptionIf(p => string.IsNullOrEmpty(p.Message), new ArgumentException($"{nameof(message.Message)} is null or empty!"));
            message.ThrowExceptionIf(p => string.IsNullOrEmpty(p.MessageId), new ArgumentException($"{nameof(message.MessageId)} is null or empty!"));

            var response = await _httpClient.Request<DefaultMicrosoftTeamsReplyMessageResponse>(
                _settings.EndpointUrl,
                HttpMethod.Post,
                message
            );

            response.ThrowExceptionIfNull<Exception>($"{nameof(response)} is null!");
            response.ThrowExceptionIf(p => string.IsNullOrEmpty(p.MessageId));

            return response;

        }

       

    }
}
