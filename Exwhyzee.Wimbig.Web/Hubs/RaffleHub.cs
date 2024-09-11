using Exwhyzee.Core.Enums;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Hubs
{
    public class RaffleHub: Hub
    {
        public async Task SendMessage(string raffleName,string user,int number,RaffleActionEnum raffleActionEnum)
        {
            await Clients.Group(raffleName).SendAsync("ReceiveMessage", user, number, raffleActionEnum);
            //await Clients.All.SendAsync(raffleName, user,number,raffleActionEnum);
        }

        public async Task JoinRaffleGroup(string raffleName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, raffleName);
        }
    }
}
