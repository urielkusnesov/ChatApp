using Microsoft.AspNet.SignalR;

namespace JobsityChat
{
    public class JobsityHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}