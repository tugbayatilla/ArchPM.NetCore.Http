namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public interface IMicrosoftTeamsLogicAppPostMessageClientSettings
    {
        string EndpointUrl { get; set; }
        bool Active { get; set; }

    }
}
