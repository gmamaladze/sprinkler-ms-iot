using System;
using System.Threading;
using System.Threading.Tasks;
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

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var telegram = TelegramRecorder.Read(18, new TimeSpan(0, 0, 0, 0, Timeout.Infinite), TimeSpan.FromMilliseconds(100), CancellationToken.None);
                    TelegramsDb.Add(telegram);
                }
            });

            _deferral = taskInstance.GetDeferral();
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<SprinklerController>();

            var telegramHandler = new RestRouteHandler();
            telegramHandler.RegisterController<TelegramsController>();

            var httpServer = new HttpServer(1390);
            httpServer.RegisterRoute("sprinkler", restRouteHandler);
            httpServer.RegisterRoute("inbox", telegramHandler);

            httpServer.StartServerAsync().Wait();
        }
    }
}