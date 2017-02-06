using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Linq;
using MongoDB.Driver.Builders;

namespace Classifieds.UserService.Repository
{
    public class UserRepository : DBRepository, IUserRepository
    {
        #region Private Variables
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["UserCollection"];
        private readonly string _collectionSubscription = ConfigurationManager.AppSettings["SubscriptionCollection"];
        private readonly IDBRepository _dbRepository;
        MongoCollection<ClassifiedsUser> Classifieds
        {
            get
            {
                return _dbRepository.GetCollection<ClassifiedsUser>(_collectionClassifieds);
            }
        }
        MongoCollection<Subscription> Subscription
        {
            get
            {
                return _dbRepository.GetCollection<Subscription>(_collectionSubscription);
            }
        }

        #endregion

        #region Constructor
        public UserRepository(IDBRepository dBRepository)
        {
            _dbRepository = dBRepository;
        }
        #endregion

        #region Public Methods

        #region RegisterUser

        /// <summary>
        /// Insert a new user object into the database
        /// </summary>
        /// <param name="user">ClassifiedsUser object</param>
        /// <returns>return newly added listing object</returns>
        public string RegisterUser(ClassifiedsUser user)
        {
            string returnStr = string.Empty;
            try
            {
                var result = this.Classifieds.FindAll()
                                .Where(p => p.UserEmail == user.UserEmail)
                                .ToList();
                if (result.Count == 0)
                {
                    var userResult = this.Classifieds.Save(user);
                    if (userResult.DocumentsAffected == 0 && userResult.HasLastErrorMessage)
                    {
                        throw new Exception("Registrtion failed");
                    }
                    returnStr = "Saved";
                }
                else
                {
                    returnStr = "Success";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnStr;
        }

        #endregion RegisterUser

        #region AddSubscription

        /// <summary>
        /// Insert a new Subscription object into the database
        /// </summary>
        /// <param name="subscriptionObj">Subscription object</param>
        /// <returns>Newly added Subscription object</returns>
        public Subscription AddSubscription(Subscription subscriptionObj)
        {
            try
            {
                var result = Subscription.Save(subscriptionObj);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }
                return subscriptionObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteSubscription
        /// <summary>
        /// Delete Subscription object based on id from the database
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>return void</returns>
        public void DeleteSubscription(string id)
        {
            try
            {
                var query = Query<Subscription>.EQ(p => p._id, id);
                Subscription.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteSubscription

        #region AddSubscriptionByCategoryandSubCategory

        /// <summary>
        /// Insert a new Subscription object into the database
        /// </summary>
        /// <param name="subscriptionObj">Subscription object</param>
        /// <returns>Newly added Subscription object</returns>
        public ClassifiedsUser AddSubscriptionByCategoryandSubCategory(ClassifiedsUser subscriptionObj)
        {
            try
            {
                var result = Classifieds.Save(subscriptionObj);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }
                return subscriptionObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion AddSubscriptionByCategoryandSubCategory

        #region DeleteubscriptionByCategoryandSubCategory
        /// <summary>
        /// Delete Subscription object based on id from the database
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>return void</returns>
        public void DeleteubscriptionByCategoryandSubCategory(string id)
        {
            try
            {
                var query = Query<ClassifiedsUser>.EQ(p => p._id, id);
                Classifieds.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteubscriptionByCategoryandSubCategory

        #endregion Public Methods
    }
}
