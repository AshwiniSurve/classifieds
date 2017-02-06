using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingRepositoryTest
    {
        #region Class Variables
        private IListingRepository<Listing> _listingRepo;
        private IDBRepository _dbRepository;
        private readonly List<Listing> _classifiedList = new List<Listing>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _listingRepo = new ListingRepository<Listing>(_dbRepository);

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
        public void Repo_GetListingByIdTest()
        {
            /*In this test case we add one post and pass recently added post's Id as a parameter to GetListingById() method instead of passing hard coded value*/
            //Arrange
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            Assert.IsNotNull(result, null);

            var recentlyAddedRecord = _listingRepo.GetListingById(result._id);

            //Assert
            Assert.AreEqual(recentlyAddedRecord.Count, 1);
        }

        /// <summary>
        /// test for incorrect id return null;
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByIdTest_NullId()
        {
            //Act
            var result = _listingRepo.GetListingById(null);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for incorrect id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_GetListingByIdTest_InvalidId_ThrowException()
        {
            _listingRepo.GetListingById("qwer");
        }

        /// <summary>
        /// test positive scenario for Get Listing By Id 
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByEmailTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingByEmail(_classifiedList[0].Submittedby);

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for incorrect email return null;
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByEmailTest_NullId()
        {
            //Act
            var result = _listingRepo.GetListingByEmail(null);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for incorrect email throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_GetListingByEmailTest_InvalidId_ThrowException()
        {
            var result = _listingRepo.GetListingByEmail("qazxsw");
            Assert.AreEqual(0, result.Count);
        }


        /// <summary>
        /// test positive scenario for get listing by category 
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsByCategory(_classifiedList[0].ListingCategory);

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for invalid category returns empty result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest_InvalidCategory()
        {
            var result = _listingRepo.GetListingsByCategory("qazxsw");
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// test for null category returns empty result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest_NullCategory()
        {
            var nullResult = _listingRepo.GetListingsByCategory(null);
            Assert.AreEqual(0, nullResult.Count);
        }

        /// <summary>
        /// test positive scenario for add listing object into the database
        /// </summary>
        [TestMethod]
        public void Repo_AddListTest()
        {
            //Arrange
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for adding empty listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_AddListTest_EmptyList_ThrowException()
        {
            _listingRepo.Add(null);
        }

        /// <summary>
        /// test positive scenario for Delete list by Id
        /// </summary>
        [TestMethod]
        public void Repo_DeleteListTest()
        {
            //Arrange
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            _listingRepo.Delete(result._id);

            var newresult = _listingRepo.GetListingById(result._id);

            //Assert
            Assert.IsNull(newresult);

        }

        /// <summary>
        /// test for incorrect id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteListTest_InvalidId_ThrowException()
        {
            _listingRepo.Delete("qwer");
        }

        /// <summary>
        /// test positive scenario for updating listing object
        /// </summary>
        [TestMethod]
        public void Repo_UpdateListTest()
        {
            //Arrange
            var lstObject = GetListObject();


            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            result.Title = "UpdatedTest";
            result.ListingCategory = "UpdatedHousing";

            var updatedresult = _listingRepo.Update(result._id, result);
            Assert.IsNotNull(updatedresult);

            Assert.AreEqual(result.Title, updatedresult.Title);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for updating listing object with null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_UpdateListTest_NullId_ThrowException()
        {
            var result = _listingRepo.Update(null, null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for get listing by sub category
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest()
        {
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            Assert.IsNotNull(result, null);

            //Act
            var newResult = _listingRepo.GetListingsBySubCategory(result.SubCategory);

            //Assert
            Assert.IsNotNull(newResult[0]);
        }

        /// <summary>
        /// test for null subcategory returns null result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_NullSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory(null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for invalid subcategory returns null result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_InvalidSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory("qwer");
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for get top 2 listing oject  
        /// </summary>
        [TestMethod]
        public void Repo_GetTopListingTest()
        {
            //Act
            var result = _listingRepo.GetTopListings(2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
        }

        /// <summary>
        /// test GetTopListing throws exception whenever.
        /// to pass this test case database server must be down.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void Repo_GetTopListingTest_ThrowException()
        {
            //Act
            _listingRepo.GetTopListings(2);
        }
        #endregion
    }
}
