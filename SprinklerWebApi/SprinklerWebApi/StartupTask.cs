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
            var db = new TelegramsDb();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    const int inputPinNumber = 18;
                    var infinite = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
                    var telegram = TelegramRecorder.Read(
                        inputPinNumber, 
                        infinite,
                        TimeSpan.FromMilliseconds(100), 
                        CancellationToken.None);
                    db.Add(DoConvert.ToDo(telegram));
                }
            });

            _deferral = taskInstance.GetDeferral();
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<SprinklerController>();

            var telegramHandler = new RestRouteHandler();
            telegramHandler.RegisterController<TelegramsController>(db);

            var httpServer = new HttpServer(1390);
            httpServer.RegisterRoute("sprinkler", restRouteHandler);
            httpServer.RegisterRoute("inbox", telegramHandler);

            httpServer.StartServerAsync().Wait();
        }
    }
}