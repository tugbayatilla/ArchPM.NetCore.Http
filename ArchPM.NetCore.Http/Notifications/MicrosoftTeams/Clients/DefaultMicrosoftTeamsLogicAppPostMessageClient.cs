﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using ArchPM.NetCore.Extensions;
using ArchPM.NetCore.Http.Extensions;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients.Settings;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;

namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public class DefaultMicrosoftTeamsLogicAppPostMessageClient : IMicrosoftTeamsLogicAppPostMessageClient
    {
        private readonly HttpClient _httpClient;
        private readonly DefaultMicrosoftTeamsLogicAppPostMessageClientSettings _settings;

        public DefaultMicrosoftTeamsLogicAppPostMessageClient(HttpClient httpClient, DefaultMicrosoftTeamsLogicAppPostMessageClientSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<IMicrosoftTeamsPostMessageResponse> SendMessage(IMicrosoftTeamsPostMessage message)
        {
            if (!_settings.Active)
            {
                return new DefaultMicrosoftTeamsPostMessageResponse();
            }

            message.ThrowExceptionIfNull<ArgumentNullException>();
            message.ThrowExceptionIf(p => string.IsNullOrEmpty(p.Message), new ArgumentException($"{nameof(message.Message)} is null or empty!"));
            message.ThrowExceptionIf(p => string.IsNullOrEmpty(p.Subject), new ArgumentException($"{nameof(message.Subject)} is null or empty!"));

            var response = await _httpClient.Request<DefaultMicrosoftTeamsPostMessageResponse>(
                p =>
                {
                    p.RequestUri = _settings.EndpointUrl;
                    p.HttpMethod = HttpMethod.Post;
                    p.Data = message;
                }
            );

            response.ThrowExceptionIfNull<Exception>($"{nameof(response)} is null!");
            response.ThrowExceptionIf(p => string.IsNullOrEmpty(p.MessageId));


            return response;
        }

       

    }
}
