using Classifieds.UserService.BusinessEntities;

namespace Classifieds.UserService.Repository
{
    public interface IUserRepository
    {
        string RegisterUser(ClassifiedsUser user);
        Subscription AddSubscription(Subscription subObject);
        void DeleteSubscription(string id);
        ClassifiedsUser AddSubscriptionByCategoryandSubCategory(ClassifiedsUser subObject);
        void DeleteubscriptionByCategoryandSubCategory(string id);
    }

}
