using Classifieds.Common;
using Classifieds.Common.Repositories;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Classifieds.UserServiceAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform user operations
    /// class name: UserController
    /// Purpose : This class is used to implement post/put methods on users
    /// Created By : Ashish
    /// Created Date: 10/01/2017
    /// Modified by :
    /// Modified date: 
    /// </summary>
    public class UserController : ApiController
    {
        #region Private Variable
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        private string _userSubcriptionEmail = string.Empty;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with injected dependencies
        /// </summary>
        public UserController(IUserService userService, ILogger logger, ICommonRepository commonRepository)
        {
            _userService = userService;
            _logger = logger;
            _commonRepository = commonRepository;
        }
        #endregion

        #region Public Methods

        #region RegisterUser 
        /// <summary>
        /// Registers the user if not present in Db
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Response containing access token</returns>
        [HttpPost]
        public HttpResponseMessage RegisterUser(ClassifiedsUser user)
        {
            string email = string.Empty;
            try
            {
                email = GetUserEmail(user);
                HttpResponseMessage response = null;
                if (user == null || user.UserEmail == null || user.UserName == null)
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid request");
                else if (!(user.UserEmail.ToLowerInvariant().EndsWith("globant.com")))
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid domain");

                var result = _userService.RegisterUser(user);

                //saving auth token
                if (result.Equals("Success") || result.Equals("Saved"))
                {
                    var tokenId = Guid.NewGuid().ToString("n");
                    Classifieds.Common.Entities.UserToken userToken = new Classifieds.Common.Entities.UserToken();
                    userToken.AccessToken = tokenId;
                    userToken.UserEmail = user.UserEmail;
                    userToken.LoginDateTime = DateTime.Now.ToString();
                    _commonRepository.SaveToken(userToken);
                    response = Request.CreateResponse<Classifieds.Common.Entities.UserToken>(HttpStatusCode.Created, userToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, email);
                throw new Exception(HttpStatusCode.Conflict.ToString() + " Internal server error");
            }
        }

        #endregion RegisterUser 

        #region AddSubscription

        /// <summary>
        /// Insert new Subscription item into the database
        /// </summary>
        /// <returns>newly added Subscription object</returns>
        public HttpResponseMessage AddSubscription(Subscription subscriptionObj)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userSubcriptionEmail = GetSubscriptionEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                var subscription = _userService.AddSubscription(subscriptionObj);
                result = Request.CreateResponse<Subscription>(HttpStatusCode.Created, subscription);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userSubcriptionEmail);
                throw;
            }

            return result;
        }

        #endregion AddSubscription

        #region DeleteSubscription
        /// <summary>
        /// Delete Subscription item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>deleted id</returns>
        public HttpResponseMessage DeleteSubscription(string id)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userSubcriptionEmail = GetSubscriptionEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                _userService.DeleteSubscription(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userSubcriptionEmail);
                throw;
            }

            return result;
        }

        #endregion

        #region AddSubscriptionByCategoryandSubCategory

        /// <summary>
        /// Insert new Subscription item into the database
        /// </summary>
        /// <returns>newly added Subscription object</returns>
        public HttpResponseMessage AddSubscriptionByCategoryandSubCategory(ClassifiedsUser subscriptionObj)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userSubcriptionEmail = GetSubscriptionEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                var classified = _userService.AddSubscriptionByCategoryandSubCategory(subscriptionObj);
                result = Request.CreateResponse<ClassifiedsUser>(HttpStatusCode.Created, classified);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userSubcriptionEmail);
                throw;
            }

            return result;
        }

        #endregion AddSubscriptionByCategoryandSubCategory

        #region DeleteSubscriptionByCategoryandSubCategory
        /// <summary>
        /// Delete Subscription item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>deleted id</returns>
        public HttpResponseMessage DeleteSubscriptionByCategoryandSubCategory(string id)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userSubcriptionEmail = GetSubscriptionEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                _userService.DeleteSubscriptionByCategoryandSubCategory(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userSubcriptionEmail);
                throw;
            }

            return result;
        }

        #endregion DeleteSubscriptionByCategoryandSubCategory

        #endregion

        #region private methods
        /// <summary>
        /// Returns user email string
        /// </summary>
        /// <param name="user">ClassifiedsUser object</param>
        /// <returns>string</returns>
        private string GetUserEmail(ClassifiedsUser user)
        {
            string result = string.Empty;
            if (user != null)
            {
                if (user.UserEmail != null)
                    result = user.UserEmail;
            }
            return result;
        }


        private string GetSubscriptionEmail()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            message.Headers.TryGetValues("UserEmail", out headerValues);
            string headerVal = headerValues == null ? string.Empty : headerValues.FirstOrDefault();
            return headerVal;
        }

        #endregion
    }
}
