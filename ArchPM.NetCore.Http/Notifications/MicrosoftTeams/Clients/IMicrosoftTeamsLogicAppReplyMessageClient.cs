using System.Threading.Tasks;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;

namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public interface IMicrosoftTeamsLogicAppReplyMessageClient
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task<IMicrosoftTeamsReplyMessageResponse> SendMessage(IMicrosoftTeamsReplyMessage message);
    }

    

    
}
