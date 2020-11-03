namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages
{
    public class DefaultMicrosoftTeamsReplyMessage : IMicrosoftTeamsReplyMessage
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
    }
}
