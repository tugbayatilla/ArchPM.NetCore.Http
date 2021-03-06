using System.Linq;
using ArchPM.NetCore.Http.Extensions;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ArchPM.NetCore.Http.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        private IConfiguration CreateConfiguration(string configFileName)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("ConfigFiles/" + configFileName)
                .Build();
        }

        [Fact]
        public void ParseConfig_Should_Be_Valid_When_Valid_Json_existing_config()
        {
            var configuration = CreateConfiguration("appsettings.valid.json");

            var settings = configuration.CreateFromAppSettings<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("PostMessageSettings");

            settings.Should().NotBeNull();
            settings.Active.Should().BeTrue();
            settings.EndpointUrl.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public void ParseConfig_Should_Be_Null_When_Valid_Json_Not_existing_config()
        {
            var configuration = CreateConfiguration("appsettings.valid.json");

            var settings = configuration.CreateFromAppSettings<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("NotExistConfig");

            settings.Should().BeNull();

        }

        [Fact]
        public void ParseConfig_Should_Be_Valid_When_missing_Json_existing_config()
        {
            var configuration = CreateConfiguration("appsettings.missing.json");

            var settings = configuration.CreateFromAppSettings<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("PostMessageSettings");

            settings.Should().BeNull();
        }

        [Fact]
        public void ParseConfig_Should_Be_Valid_When_structured_Json_existing_config()
        {
            var configuration = CreateConfiguration("appsettings.valid.structured.json");

            var settings = configuration.CreateFromAppSettings<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("Settings:PostMessageSettings");

            settings.Should().NotBeNull();
            settings.Active.Should().BeTrue();
            settings.EndpointUrl.Should().Be("https://loremipsum.com/api/v2/post");
        }


        private IServiceCollection CreateServiceCollection()
        {
            var service = new ServiceCollection();
            service.AddSingleton(CreateConfiguration("appsettings.valid.json"));

            return service;
        }


        [Fact]
        public void AddDefaultMicrosoftTeamsClientSettings_Should_throw_exception_when_invalid_config_for_post()
        {
            var service = CreateServiceCollection();

            service.AddDefaultMicrosoftTeamsClientSettings();


            var postSettings = service.ToList()
                .FirstOrDefault(
                    p => p.ServiceType ==
                        typeof(DefaultMicrosoftTeamsLogicAppPostMessageClientSettings)
                );
            postSettings.Should().NotBeNull();
            postSettings?.Lifetime.Should().Be(ServiceLifetime.Singleton);
           

            var replySettings = service.ToList()
                .FirstOrDefault(
                    p => p.ServiceType ==
                        typeof(DefaultMicrosoftTeamsLogicAppReplyMessageClientSettings)
                );

            replySettings.Should().NotBeNull();
            replySettings?.Lifetime.Should().Be(ServiceLifetime.Singleton);

            var postClient = service.ToList()
                .FirstOrDefault(
                    p => p.ServiceType ==
                        typeof(IMicrosoftTeamsLogicAppPostMessageClient)
                );
            postClient.Should().NotBeNull();
            postClient?.Lifetime.Should().Be(ServiceLifetime.Transient);

            var replyClient = service.ToList()
                .FirstOrDefault(
                    p => p.ServiceType ==
                        typeof(IMicrosoftTeamsLogicAppReplyMessageClient)
                );
            replyClient.Should().NotBeNull();
            replyClient?.Lifetime.Should().Be(ServiceLifetime.Transient);


            using (var provider = service.BuildServiceProvider())
            {
                var postSettingsInstance = provider.GetService<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>();
                postSettingsInstance.Active.Should().BeTrue();
                postSettingsInstance.EndpointUrl.Should().Be("https://loremipsum.com/api/v2/post");

                var replySettingsInstance = provider.GetService<DefaultMicrosoftTeamsLogicAppReplyMessageClientSettings>();
                replySettingsInstance.Active.Should().BeTrue();
                replySettingsInstance.EndpointUrl.Should().Be("https://loremipsum.com/api/v2/reply");
            }

        }

        [Fact]
        public void AddDefaultMicrosoftTeamsClientSettings_Should_work_with_config_when_use_post_is_false()
        {
            var service = CreateServiceCollection();

            service.AddDefaultMicrosoftTeamsClientSettings(
                p => { p.UsePostMessageClient = false; });


            var postSettings = service.ToList()
                .FirstOrDefault(
                    p => p.ServiceType ==
                        typeof(DefaultMicrosoftTeamsLogicAppPostMessageClientSettings)
                );
            postSettings.Should().BeNull();

            var postClient = service.ToList()
                .FirstOrDefault(
                    p => p.ServiceType ==
                        typeof(IMicrosoftTeamsLogicAppPostMessageClient)
                );
            postClient.Should().BeNull();

            using (var provider = service.BuildServiceProvider())
            {
                var postSettingsInstance = provider.GetService<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>();
                postSettingsInstance.Should().BeNull();
            }

        }
    }
}
