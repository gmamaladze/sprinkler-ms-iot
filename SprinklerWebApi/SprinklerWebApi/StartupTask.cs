using Windows.ApplicationModel.Background;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SprinklerWebApi
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<SprinklerController>();

            var httpServer = new HttpServer(1390);
            httpServer.RegisterRoute("sprinkler", restRouteHandler);
            httpServer.StartServerAsync().Wait();
        }
    }
}