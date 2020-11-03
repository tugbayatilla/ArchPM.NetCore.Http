namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public interface IMicrosoftTeamsLogicAppReplyMessageClientSettings
    {
        string EndpointUrl { get; set; }
        bool Active { get; set; }

    }
}
