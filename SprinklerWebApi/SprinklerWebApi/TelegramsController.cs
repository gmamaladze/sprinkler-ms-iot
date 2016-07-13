using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace SprinklerWebApi
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class TelegramsController
    {
        private readonly TelegramsDb _db;

        public TelegramsController(TelegramsDb db)
        {
            _db = db;
        }

        [UriFormat("/{id}")]
        public IGetResponse Get(int id)
        {
            var telegram = _db.Get(id);
            return telegram==null 
                ? new GetResponse(GetResponse.ResponseStatus.NotFound) 
                : new GetResponse(GetResponse.ResponseStatus.OK, telegram);
        }

        [UriFormat("/all")]
        public IGetResponse GetAll()
        {
            var telegrams = _db.GetAll();
            return new GetResponse(GetResponse.ResponseStatus.OK, telegrams);
        }

        [UriFormat("/{id}")]
        public IDeleteResponse Delete(int id)
        {
            var deleted = _db.Delete(id);
            return new DeleteResponse(deleted>0 ? DeleteResponse.ResponseStatus.OK : DeleteResponse.ResponseStatus.NotFound);
        }

        [UriFormat("/clear")]
        public IGetResponse GetClear()
        {
            var telegrams = _db.GetAll();
            _db.Clear();
            return new GetResponse(GetResponse.ResponseStatus.OK, telegrams);
        }
    }
}