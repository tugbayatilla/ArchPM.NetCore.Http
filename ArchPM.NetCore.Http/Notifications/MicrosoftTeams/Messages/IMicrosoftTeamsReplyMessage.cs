namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages
{
    public interface IMicrosoftTeamsReplyMessage
    {
        string MessageId { get; set; }
        string Message { get; set; }
    }
}
