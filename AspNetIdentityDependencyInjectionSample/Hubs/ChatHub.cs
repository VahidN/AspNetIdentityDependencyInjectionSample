using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AspNetIdentityDependencyInjectionSample.Hubs
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        private readonly IApplicationUserManager _userManager;

        public ChatHub(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public void Hello()
        {
            Clients.All.hello(string.Format("Helo from: {0}", _userManager.GetCurrentUser().UserName));
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            var user = _userManager.GetCurrentUser();
            var msg = string.Format("Helo from: {0}", user.UserName);
            Clients.Client(Context.ConnectionId).hello(msg);
            return base.OnConnected();
        }
    }
}