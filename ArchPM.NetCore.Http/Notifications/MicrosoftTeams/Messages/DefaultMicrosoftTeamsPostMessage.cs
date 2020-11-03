namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages
{
    public class DefaultMicrosoftTeamsPostMessage : IMicrosoftTeamsPostMessage
    {
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
