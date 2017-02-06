using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using Classifieds.Listings.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingBusinessServiceTest
    {
        #region Class Variables
        private Mock<IListingRepository<Listing>> _moqAppManager;
        private IListingService _service;
        private readonly List<Listing> _classifiedList = new List<Listing>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IListingRepository<Listing>>();
            _service = new ListingService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsListing()
        {
            var lstListing = GetListObject();
            _classifiedList.Add(lstListing);            
        }

        private Listing GetListObject()
        {
            var listObject = new Listing
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
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingById(It.IsAny<string>())).Returns(_classifiedList);

            //Act
            var result = _service.GetListingById(_classifiedList[0]._id);

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for empty result  i.e. no match found
        /// </summary>
        [TestMethod]
        public void GetListingById_EmptyResult_Test()
        {
            //Arrange
            var lstObject = GetListObject();
            _moqAppManager.Setup(x => x.GetListingById(It.IsAny<string>())).Returns(new List<Listing>() );

            //Act
            var result = _service.GetListingById(lstObject._id);

            //Assert
            Assert.AreEqual(result.Count,0);           
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingById_ThrowsException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetListingById(null)).Throws(ex);
            _service.GetListingById(null);
        }

        /// <summary>
        /// test positive scenario for Get Listing By Email  
        /// </summary>
        [TestMethod]
        public void GetListingByEmailTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingByEmail(It.IsAny<string>())).Returns(_classifiedList);

            //Act
            var result = _service.GetListingByEmail(_classifiedList[0].Submittedby);

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for empty result  i.e. no match found
        /// </summary>
        [TestMethod]
        public void GetListingByEmail_EmptyResult_Test()
        {
            //Arrange
            var lstObject = GetListObject();
            _moqAppManager.Setup(x => x.GetListingByEmail(It.IsAny<string>())).Returns(new List<Listing>());

            //Act
            var result = _service.GetListingByEmail(lstObject.Submittedby);

            //Assert
            Assert.AreEqual(result.Count, 0);
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingByEmail_ThrowsException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetListingByEmail(null)).Throws(ex);
            _service.GetListingByEmail(null);
        }

        /// <summary>
        /// test positive scenario for GetListingsBySubCategory  
        /// </summary>
        [TestMethod]
        public void GetListingsBySubCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingsBySubCategory(It.IsAny<string>())).Returns(_classifiedList);

            //Act
            var result = _service.GetListingsBySubCategory(_classifiedList[0].SubCategory);

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingsBySubCategory_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetListingsBySubCategory(null)).Throws(ex);
            _service.GetListingsBySubCategory(null);
        }

        /// <summary>
        /// test for empty result i.e. no match found
        /// </summary>
        [TestMethod]
        public void GetListingsBySubCategory_EmptyResult_Test()
        {
            // Arrange
            var lstObject = GetListObject();
            _moqAppManager.Setup(x => x.GetListingsBySubCategory(It.IsAny<string>())).Returns(new List<Listing>());

            //Act
            var result = _service.GetListingsBySubCategory(lstObject.SubCategory);

            //Assert
            Assert.AreEqual(result.Count,0);
            Assert.IsInstanceOfType(result, typeof(IList<Listing>));
        }
        
        /// <summary>
        /// tests the positive test criteria
        /// </summary>
        [TestMethod]
        public void GetListingByCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingsByCategory(It.IsAny<string>())).Returns(_classifiedList);

            //Act
            var result = _service.GetListingsByCategory(_classifiedList[0].ListingCategory);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// tests for giving empty result  i.e. no match found
        /// </summary>
        [TestMethod]
        public void GetListingByCategory_EmptyResultTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingsByCategory(It.IsAny<string>())).Returns(new List<Listing>());

            //Act
            var result = _service.GetListingsByCategory(_classifiedList[0].ListingCategory);

            //Assert
            Assert.AreEqual(result.Count,0);
        }

        /// <summary>
        /// tests for null category input throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingByCategory_ThrowException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetListingsByCategory(null)).Throws(ex);            
            _service.GetListingsByCategory(null);
        }

        /// <summary>
        /// test positive scenario for PostList
        /// </summary>
        [TestMethod]
        public void PostListTest()
        {
            //Arrange
            var lstObject = GetListObject();
            _moqAppManager.Setup(x => x.Add(It.IsAny<Listing>())).Returns(lstObject);

            //Act
            var result = _service.CreateListing(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for inserting empty listing object return null result
        /// </summary>
        [TestMethod]
        public void PostListTest_EmptyList()
        {
            //Arrange
            var list = new Listing();
            _moqAppManager.Setup(x => x.Add(It.IsAny<Listing>())).Returns((new Listing()));
            //Act
            var result = _service.CreateListing(list);
            //Assert
            Assert.AreEqual(result.Title, null);
        }

        /// <summary>
        /// test for inserting null listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PostListTest_ThrowException()
        {
            //Arrange
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.Add(null)).Throws(ex);
            //Act
            var result = _service.CreateListing(null);
            //Assert
            Assert.IsNull(result, null);
        }

        /// <summary>
        /// test positive scenario for DeleteList
        /// </summary>
        [TestMethod]
        public void DeleteListTest()
        {
            //Arrange
            var lstObject = GetListObject();
            _moqAppManager.Setup(x => x.Delete(It.IsAny<string>()));

            //Act
            _service.DeleteListing(lstObject._id);

            //Assert
            Assert.IsTrue(true);
            _moqAppManager.Verify(v => v.Delete(lstObject._id), Times.Once());
        }

        /// <summary>
        /// test for delete listing with null id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteListTest_InvalidId_ThrowException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.Delete(null)).Throws(ex);
            _service.DeleteListing(null);
        }

        /// <summary>
        /// test positive scenario for update listing
        /// </summary>
        [TestMethod]
        public void PutListTest()
        {
            //Arrange
            var lstObject = GetListObject();
            _moqAppManager.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Listing>())).Returns(lstObject);
            var updatedList = new Listing() { Title = lstObject.Title, ListingType = lstObject.ListingType };
            //Act
            var result = _service.UpdateListing(lstObject._id, updatedList);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for updating listing with null id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PutListTest_InvalidId_ThrowException()
        {
            //Arrange
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Listing>())).Throws(ex);

            //Act
            _service.UpdateListing(null, null);
        }

        /// <summary>
        /// test for positive scenario for get top listing
        /// </summary>
        [TestMethod]
        public void GetTopListing_5RecordsTest()
        {
            // Arrange
            List<Listing> list = new List<Listing>();
            for (var i = 0; i < 5; i++)
            {
                list.Add(GetListObject());
            }
            _moqAppManager.Setup(x => x.GetTopListings(It.IsAny<int>())).Returns(list);
            
            //Act
            var result = _service.GetTopListings(5);

            //Assert
            Assert.AreEqual(result.Count, 5);
        }

        /// <summary>
        /// test for throwing exception in GetTopListing
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTopListing_ThrowException()
        {
            // Arrange
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());           
            _moqAppManager.Setup(x => x.GetTopListings(It.IsAny<int>())).Throws(ex);

            //Act
            _service.GetTopListings(5);           
        }
        #endregion
    }
}
