using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using Windows.Devices.Gpio;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SprinklerWebApi
{

    [RestController(InstanceCreationType.Singleton)]
    public sealed class SprinklerController
    {
        private CancellationTokenSource _ctx;

        [UriFormat("/{pin}/on/{seconds}")]
        public IGetResponse GetOn(int pinId, int seconds)
        {
            _ctx?.Cancel();
            _ctx = new CancellationTokenSource();

            var gpio = GpioController.GetDefault();
            var pin = gpio.OpenPin(pinId);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            pin.Write(GpioPinValue.High);
          
            Task.Factory.StartNew( () =>
            {
              
            });
            

            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                DateTime.UtcNow.AddSeconds(seconds));
        }

        public IGetResponse GetOff(int pin)
        {
        }
    }
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

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
