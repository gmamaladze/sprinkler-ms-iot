using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace SprinklerWebApi
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class TelegramsController
    {
        [UriFormat("/{id}")]
        public IGetResponse Get(int id)
        {
            var telegram = TelegramsDb.Get(id);
            return telegram==null 
                ? new GetResponse(GetResponse.ResponseStatus.NotFound) 
                : new GetResponse(GetResponse.ResponseStatus.OK, telegram);
        }

        [UriFormat("/all/")]
        public IGetResponse Get()
        {
            var telegrams = TelegramsDb.GetAll();
            return new GetResponse(GetResponse.ResponseStatus.OK, telegrams);
        }

        [UriFormat("/{id}")]
        public IDeleteResponse Delete(int id)
        {
            var deleted = TelegramsDb.Delete(id);
            return new DeleteResponse(deleted>0 ? DeleteResponse.ResponseStatus.OK : DeleteResponse.ResponseStatus.NotFound);
        }

        [UriFormat("/clear/")]
        public IGetResponse Clear()
        {
            var deleted = TelegramsDb.Clear();
            return new GetResponse(GetResponse.ResponseStatus.OK, deleted);
        }
    }
}