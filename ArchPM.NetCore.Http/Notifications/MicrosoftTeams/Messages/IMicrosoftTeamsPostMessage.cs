namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages
{
    public interface IMicrosoftTeamsPostMessage
    {
        string Subject { get; set; }
        string Message { get; set; }
    }
}
