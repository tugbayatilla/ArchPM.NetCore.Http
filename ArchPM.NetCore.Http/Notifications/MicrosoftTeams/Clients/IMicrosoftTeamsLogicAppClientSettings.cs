namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public interface IMicrosoftTeamsLogicAppClientSettings
    {
        string EndpointUrl { get; set; }
        bool Active { get; set; }

    }
}
