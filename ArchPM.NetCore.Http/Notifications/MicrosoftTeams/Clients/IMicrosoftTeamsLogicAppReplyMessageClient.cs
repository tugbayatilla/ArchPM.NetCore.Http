using System.Threading.Tasks;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;

namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public interface IMicrosoftTeamsLogicAppReplyMessageClient
    {
        Task<IMicrosoftTeamsReplyMessageResponse> SendMessage(IMicrosoftTeamsReplyMessage message);
    }

    

    
}
