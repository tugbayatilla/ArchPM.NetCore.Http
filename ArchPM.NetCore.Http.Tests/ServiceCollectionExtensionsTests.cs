using ArchPM.NetCore.Http.Extensions;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
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

            var settings = configuration.ParseConfig<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("PostMessageSettings");

            settings.Should().NotBeNull();
            settings.Active.Should().BeTrue();
            settings.EndpointUrl.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public void ParseConfig_Should_Be_Null_When_Valid_Json_Not_existing_config()
        {
            var configuration = CreateConfiguration("appsettings.valid.json");

            var settings = configuration.ParseConfig<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("NotExistConfig");

            settings.Should().BeNull();

        }

        [Fact]
        public void ParseConfig_Should_Be_Valid_When_missing_Json_existing_config()
        {
            var configuration = CreateConfiguration("appsettings.missing.json");

            var settings = configuration.ParseConfig<DefaultMicrosoftTeamsLogicAppPostMessageClientSettings>("PostMessageSettings");

            settings.Should().BeNull();
        }

    }
}
