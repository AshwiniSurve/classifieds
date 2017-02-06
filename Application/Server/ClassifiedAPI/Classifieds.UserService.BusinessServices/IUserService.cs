using Classifieds.UserService.BusinessEntities;

namespace Classifieds.UserService.BusinessServices
{
    public interface IUserService
    {
        string RegisterUser(ClassifiedsUser user);
        Subscription AddSubscription(Subscription subObject);
        void DeleteSubscription(string id);
        ClassifiedsUser AddSubscriptionByCategoryandSubCategory(ClassifiedsUser subObject);
        void DeleteubscriptionByCategoryandSubCategory(string id);
    }
}
