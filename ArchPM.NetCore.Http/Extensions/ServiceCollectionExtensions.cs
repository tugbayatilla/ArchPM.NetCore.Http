using System;
using ArchPM.NetCore.Extensions;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArchPM.NetCore.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDefaultMicrosoftTeamsClientSettings(this IServiceCollection services, Action<ClientConfigSettings> configSettings = null)
        {
            var settings = new ClientConfigSettings();
            configSettings?.Invoke(settings);

            using (var provider = services.BuildServiceProvider())
            {
                var configuration = provider.GetService<IConfiguration>();

                if (settings.UsePostMessageClient)
                {
                    var postSettingsObj =
                        ParseConfig<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>(
                            configuration,
                            settings.PostMessageClientConfigSectionName
                        );
                    postSettingsObj.ThrowExceptionIfNull<Exception>(
                        $"{nameof(settings.PostMessageClientConfigSectionName)} section could not be loaded into {nameof(DefaultMicrosoftTeamsLogicAppPostMessageClientSettings)}!"
                    );
                    services.AddSingleton(postSettingsObj);
                    services
                        .AddHttpClient<IMicrosoftTeamsLogicAppPostMessageClient,
                            DefaultMicrosoftTeamsLogicAppPostMessageClient>();
                }

                if (settings.UseReplyMessageClient)
                {
                    var replySettingsObj =
                        ParseConfig<DefaultMicrosoftTeamsLogicAppReplyMessageClientSettings>(
                            configuration,
                            settings.ReplyMessageClientConfigSectionName
                        );
                    replySettingsObj.ThrowExceptionIfNull<Exception>(
                        $"{nameof(settings.ReplyMessageClientConfigSectionName)} section could not be loaded into {nameof(DefaultMicrosoftTeamsLogicAppReplyMessageClientSettings)}!"
                    );

                    services.AddSingleton(replySettingsObj);
                    services
                        .AddHttpClient<IMicrosoftTeamsLogicAppReplyMessageClient,
                            DefaultMicrosoftTeamsLogicAppReplyMessageClient>();
                }
            }
        }

        public class ClientConfigSettings
        {
            public bool UsePostMessageClient { get; set; } = true;
            public bool UseReplyMessageClient { get; set; } = true;
            public string PostMessageClientConfigSectionName { get; set; } = "PostMessageSettings";
            public string ReplyMessageClientConfigSectionName { get; set; } = "ReplyMessageSettings";
        }

        /// <summary>
        /// Parses the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static T ParseConfig<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var section = configuration.GetSection(sectionName);
            if (!section.Exists())
            {
                return default;
            }

            var entity = new T();
            var properties = entity.CollectProperties(p => p.IsPrimitive);
            foreach (var propertyDto in properties)
            {
                try
                {
                    entity.SetValue(propertyDto.Name, section[propertyDto.Name]);
                }
                catch
                {
                    // ignored
                }
            }

            return entity;
        }
    }
}
