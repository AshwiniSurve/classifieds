using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using System.Collections.Generic;
using Classifieds.ListingsAPI.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;
using Classifieds.Common.Repositories;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingControllerTest
    {
        #region Class Variables
        private Mock<IListingService> _mockService;
        private Mock<ICommonRepository> _mockAuthRepo;
        private Mock<ILogger> _logger;
        private readonly List<Listing> _classifiedList = new List<Listing>();
        private const string UrlLocation = "http://localhost/api/listings";
        private ListingsController _controller;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<IListingService>();
            _logger = new Mock<ILogger>();
            _mockAuthRepo = new Mock<ICommonRepository>();
            _controller = new ListingsController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
        }
        #endregion

        #region Setup Methods
        private void SetUpClassifiedsListing()
        {
            var lstListing = GetListObject();
            _classifiedList.Add(lstListing);
        }

        private Listing GetListObject()
        {
            Listing listObject = new Listing
            {
                _id = "9",
                ListingType = "sale",
                ListingCategory = "Housing",
                SubCategory = "3 bhk",
                Title = "flat on rent",
                Address = "Pune",
                ContactNo = "12345",
                ContactName = "varun wadsamudrakar",
                Configuration = "NA",
                Details = "for rupees 22,000",
                Brand = "New",
                Price = 4500000,
                YearOfPurchase = 2000,
                ExpiryDate = "03-02-2018",
                Status = "ok",
                Submittedby = "v.wadsamudrakar@globant.com",
                SubmittedDate = "03-02-2017",
                IdealFor = "Family",
                Furnished = "Yes",
                FuelType = "test",
                KmDriven = 0000,
                YearofMake = 2016,
                Dimensions = "test",
                TypeofUse = "test",
                Photos = new[] { "/Photos/flat2016.jpg", "/Photos/flat2016.jpg" }
            };
            return listObject;


        }
        #endregion

        #region Unit Test Cases
        /// <summary>
        /// test positive scenario for Get Listing By Id  
        /// </summary>
        [TestMethod]
        public void GetListingByIdTest()
        {
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetListingById(It.IsAny<string>()))
                .Returns(_classifiedList);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act           
            var objList = _controller.GetListingById("123");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].Title, "test");
        }

        /// <summary>
        /// test positive scenario for Get Listing By Email  
        /// </summary>
        [TestMethod]
        public void GetListingByEmailTest()
        {
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetListingByEmail(It.IsAny<string>()))
                .Returns(_classifiedList);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act           
            var objList = _controller.GetListingByEmail("v.wadsamudrakar@globant.com");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].Submittedby, "v.wadsamudrakar@globant.com");
        }

        /// <summary>
        ///test positive scenario for Get Listings By SubCategory 
        /// </summary>
        [TestMethod]
        public void GetListingsBySubCategoryTest()
        {
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetListingsBySubCategory(It.IsAny<string>()))
                .Returns(_classifiedList);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act            
            var objList = _controller.GetListingsBySubCategory("test");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubCategory, "test");
        }

        /// <summary>
        /// test for null listing id giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetListingById_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetListingById(null);
        }

        /// <summary>
        /// test for null listing Email giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetListingByEmail_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetListingByEmail(null);
        }

        /// <summary>
        /// test for null subcategory giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetListingsBySubCategory_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetListingsBySubCategory(null);
        }

        /// <summary>
        /// test positive scenario get listing collection by category
        /// </summary>
        [TestMethod]
        public void GetListingsByCategoryTest()
        {
            //Arrange            
            SetUpClassifiedsListing();

            //Act
            _mockService.Setup(service => service.GetListingsByCategory(It.IsAny<string>())).Returns(_classifiedList);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            var values = _controller.GetListingsByCategory("Housing");

            //Assert
            Assert.AreEqual(values.Count, 1);
            Assert.AreEqual(values[0], _classifiedList[0]);
        }

        /// <summary>
        ///  test for null category giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingByCategory_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetListingsByCategory(null);
        }

        /// <summary>
        /// test positive scenario for PostList and verify response header location
        /// </summary>
        [TestMethod]
        public void Controller_PostListTest_SetsLocationHeader()
        {
            // Arrange
            _mockService.Setup(x => x.CreateListing(It.IsAny<Listing>()))
                .Returns(GetListObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(UrlLocation)
            };
            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "Listings",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "listings" } });

            // Act
            Listing listObj = GetListObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(UrlLocation + "/9", response.Headers.Location.AbsoluteUri);
        }

        /// <summary>
        /// test positive scenario for postlist by using mock url helper
        /// </summary>
        [TestMethod]
        public void Controller_PostListTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange           
            _mockService.Setup(x => x.CreateListing(It.IsAny<Listing>()))
                .Returns(GetListObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/ListingsAPI/api/listings";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _controller.Url = mockUrlHelper.Object;

            // Act
            Listing listObj = GetListObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for inserting null listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_PostList_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Post(null);
        }

        /// <summary>
        /// test positive scenario of Delete listing
        /// </summary>     
        [TestMethod]
        public void Controller_DeleteListTest()
        {
            // Arrange
            Listing listObject = GetListObject();
            _mockService.Setup(x => x.DeleteListing(It.IsAny<string>()));//.Returns(GetListObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(UrlLocation)
            };
            // Act                
            var response = _controller.Delete(listObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteList_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Delete(null);
        }

        /// <summary>
        /// test positive scenario for updating listing
        /// </summary>
        [TestMethod]
        public void Controller_UpdateListTest()
        {
            // Arrange
            _mockService.Setup(x => x.UpdateListing(It.IsAny<string>(), It.IsAny<Listing>()))
                .Returns(GetListObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(UrlLocation)
            };
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var listObject = GetListObject();
            var updatedProduct = new Listing() { Title = listObject.Title, ListingType = listObject.ListingType };
            var contentResult = _controller.Put(listObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            //Assert.That(listObject._id, Is.EqualTo("9")); // hasn't changed
        }

        /// <summary>
        ///  test for updating listing with null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateList_ThrowsException()
        {
            var updatedProduct = new Listing() { Title = "test", ListingType = "test" };
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Put(null, updatedProduct);
        }

        /// <summary>
        /// test positive scenario for get top listing collection as per specyfied record count
        /// </summary>
        [TestMethod]
        public void GetTopListings_5RecordsTest()
        {
            //Arrange
            List<Listing> list = new List<Listing>();
            for (int i = 0; i < 5; i++) 
            {
                list.Add(GetListObject());
            }
            _mockService.Setup(x => x.GetTopListings(It.IsAny<int>())).Returns(list);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act            
            var objList = _controller.GetTopListings(5);

            //Assert
            Assert.AreEqual(objList.Count, 5);
        }

        /// <summary>
        /// test positive scenario for get top listing, by default returns 10 records
        /// </summary>
        [TestMethod]
        public void GetTopListings_Defualt_noOfRecords_Test()
        {
            //Arrange
            List<Listing> list = new List<Listing>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(GetListObject());
            }
            _mockService.Setup(x => x.GetTopListings(It.IsAny<int>())).Returns(list);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            var objList = _controller.GetTopListings();

            //Assert
            Assert.AreEqual(objList.Count, 10);
        }       
        #endregion
    }
}
