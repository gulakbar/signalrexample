using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;

namespace SignalRSelfHost
{
    class Program
    {
		public static MyHub myHub;

        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

				string inputFromServer;

				while ((inputFromServer = Console.ReadLine()) != string.Empty)
				{
					myHub.Send("myHub", inputFromServer);
					myHub.IdentifySelectedPatient();
				}
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    public class MyHub : Hub
    {
		public MyHub()
		{
			Program.myHub = this;

		}
		
		public void Send(string name, string message)
        {
			Console.WriteLine(name + "    " + message);
            Clients.All.addMessage(name, message);
        }

	
		public void SendIdentifier(string selectedpatient, string name)
		{
			Console.WriteLine(selectedpatient + "    " + name);
		}

		public void IdentifySelectedPatient()
		{
			Clients.All.identifySelf();
		}
    }
}