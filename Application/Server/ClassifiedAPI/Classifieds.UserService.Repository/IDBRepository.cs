using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;

namespace Classifieds.UserService.Repository
{
    public interface IDBRepository
    {
        MongoCollection<ClassifiedsUser> GetCollection<ClassifiedsUser>(string name);
        MongoCollection<Subscription> GetSubscription<Subscription>(string name);
    }
}
