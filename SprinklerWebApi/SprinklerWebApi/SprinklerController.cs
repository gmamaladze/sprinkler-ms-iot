using System;
using System.Threading;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace SprinklerWebApi
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class SprinklerController
    {
        private CancellationTokenSource _ctx;

        [UriFormat("/{pinId}/on/{seconds}")]
        public IGetResponse GetOn(int pinId, int seconds)
        {
            _ctx?.Cancel();
            _ctx = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                var device = new Rc433MhzSwitch(pinId, "101110A");
                device.On();
                var t = Task.Delay(TimeSpan.FromSeconds(seconds), _ctx.Token);
                t.Wait();
                if (t.IsCanceled) return;
                device.Off();
            });

            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                DateTime.UtcNow.AddSeconds(seconds));
        }

        [UriFormat("/{pinId}/moisture")]
        public IGetResponse GetMoisture(int pinId)
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new[] {1.0, 1.0, 1.0, 1.0});
        }
    }
}