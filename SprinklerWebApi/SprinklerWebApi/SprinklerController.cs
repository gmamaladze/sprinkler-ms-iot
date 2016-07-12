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
        [UriFormat("/{pinId}/on")]
        public IGetResponse GetOn(int pinId)
        {
            var cmd = new Command(42, 0, true, false);
            Signal.Send(pinId, cmd.Data);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/{pinId}/off")]
        public IGetResponse GetOff(int pinId)
        {
            var cmd = new Command(42, 0, false, false);
            Signal.Send(pinId, cmd.Data);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }


        [UriFormat("/{pinId}/learn")]
        public IGetResponse GetLearn(int pinId)
        {
            var cmd = new Command(42, 0, true, false);
            for (var i = 0; i < 100; i++)
            {
                Signal.Send(pinId, cmd.Data);
            }

            return new GetResponse(GetResponse.ResponseStatus.OK);
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