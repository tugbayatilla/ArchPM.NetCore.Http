using System.Threading.Tasks;
using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;

namespace ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients
{
    public interface IMicrosoftTeamsLogicAppPostMessageClient
    {
        Task<IMicrosoftTeamsPostMessageResponse> SendMessage(IMicrosoftTeamsPostMessage message);
    }

   

   
}
