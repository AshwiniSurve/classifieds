using System;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.Repository;

namespace Classifieds.UserService.BusinessServices
{
    public class UserService : IUserService
    {
        #region Private Variables
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Public Methods

        #region RegisterUser
        /// <summary>
        /// Registers a classifieds user into the database
        /// </summary>
        /// <param name="user">ClassifiedsUser Object</param>
        /// <returns></returns>
        public string RegisterUser(ClassifiedsUser user)
        {
            try
            {
                return _userRepository.RegisterUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion RegisterUser

        #region AddSubscription

        /// <summary>
        /// Insert new Subscription item into the database
        /// </summary>
        /// <param name="subscriptionObj">Subscription Object</param>
        /// <returns>Newly added Subscription object</returns>
        public Subscription AddSubscription(Subscription subscriptionObj)
        {
            try
            {
                return _userRepository.AddSubscription(subscriptionObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteSubscription
        /// <summary>
        /// Delete Subscription item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>deleted Id</returns>
        public void DeleteSubscription(string id)
        {
            try
            {
                _userRepository.DeleteSubscription(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteSubscription

        #region AddSubscriptionByCategoryandSubCategory

        /// <summary>
        /// Insert new Subscription item into the database
        /// </summary>
        /// <param name="subscriptionObj">Subscription Object</param>
        /// <returns>Newly added Subscription object</returns>
        public ClassifiedsUser AddSubscriptionByCategoryandSubCategory(ClassifiedsUser subscriptionObj)
        {
            try
            {
                return _userRepository.AddSubscriptionByCategoryandSubCategory(subscriptionObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion AddSubscriptionByCategoryandSubCategory

        #region DeleteSubscriptionByCategoryandSubCategory
        /// <summary>
        /// Delete Subscription item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>deleted Id</returns>
        public void DeleteSubscriptionByCategoryandSubCategory(string id)
        {
            try
            {
                _userRepository.DeleteSubscriptionByCategoryandSubCategory(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteSubscriptionByCategoryandSubCategory

        #endregion
    }
}
