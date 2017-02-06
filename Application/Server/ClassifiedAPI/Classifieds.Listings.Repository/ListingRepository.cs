using Classifieds.Listings.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Classifieds.Listings.Repository
{
    public class ListingRepository<TEntity> : DBRepository, IListingRepository<TEntity> where TEntity:Listing
    {
        #region Private Variables
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["ListingCollection"];        
        private readonly IDBRepository _dbRepository;
        private MongoCollection<TEntity> Classifieds
        {
            get { return _dbRepository.GetCollection<TEntity>(_collectionClassifieds); }
        }
        #endregion

        #region Constructor
        public ListingRepository(IDBRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a listing based on listing id
        /// </summary>
        /// <param name="id">listing id</param>
        /// <returns>listing</returns>
        public List<TEntity> GetListingById(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var partialRresult = Classifieds.Find(query)//.Where(p => p._id == id)
                                        .ToList();

                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns a listing based on listing Email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Listing Email</returns>
        public List<TEntity> GetListingsByEmail(string email)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p.Submittedby, email);
                var partialRresult = Classifieds.Find(query)
                                        .ToList();

                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Returns a collection of listings based on sub category
        /// </summary>
        /// <param name="subCategory">listing Sub Category</param>
        /// <returns>Collection of listings</returns>
        public List<TEntity> GetListingsBySubCategory(string subCategory)
        {
            try
            {
                var partialRresult = Classifieds.FindAll()
                                        .Where(p => p.SubCategory == subCategory)
                                        .ToList();

                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns a collection of listings based on category
        /// </summary>
        /// <param name="category">listing category</param>
        /// <returns>Collection of listings</returns>
        public List<TEntity> GetListingsByCategory(string category)
        {
            try
            {
                List<TEntity> result = Classifieds.FindAll()
                                            .Where(p => p.ListingCategory == category)
                                            .ToList();
                return result;
            }
            catch (MongoException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert a new listing object into the database
        /// </summary>
        /// <param name="listing">listing object</param>
        /// <returns>return newly added listing object</returns>
        public TEntity Add(TEntity listing)
        {
            try
            {
                var result = Classifieds.Save(listing);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage){ }
                return listing;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update existing listing object based on id from the database
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listObj">listing object </param>
        /// <returns>return updated listing object</returns>
        public TEntity Update(string id, TEntity listObj)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var update = Update<TEntity>.Set(p => p.Title, listObj.Title)
                                             .Set(p => p.ListingType, listObj.ListingType)
                                             .Set(p => p.ListingCategory, listObj.ListingCategory)
                                             .Set(p => p.Brand, listObj.Brand)
                                             .Set(p => p.Price, listObj.Price)
                                             .Set(p => p.YearOfPurchase, listObj.YearOfPurchase)
                                             .Set(p => p.ExpiryDate, listObj.ExpiryDate)
                                             .Set(p => p.Status, listObj.Status)
                                             .Set(p => p.Submittedby, listObj.Submittedby)
                                             .Set(p => p.SubmittedDate, listObj.SubmittedDate)
                                             .Set(p => p.IdealFor, listObj.IdealFor)
                                             .Set(p => p.Furnished, listObj.Furnished)
                                             .Set(p => p.FuelType, listObj.FuelType)
                                             .Set(p => p.KmDriven, listObj.KmDriven)
                                             .Set(p => p.YearofMake, listObj.YearofMake)
                                             .Set(p => p.Dimensions, listObj.Dimensions)
                                             .Set(p => p.TypeofUse, listObj.TypeofUse)
                                             .Set(p => p.Photos, listObj.Photos)
                                             .Set(p => p.Address, listObj.Address)
                                             .Set(p => p.ContactName, listObj.ContactName)
                                             .Set(p => p.ContactNo, listObj.ContactNo)
                                             .Set(p => p.Details, listObj.Details)
                                             .Set(p => p.Configuration, listObj.Configuration);


                var result = Classifieds.Update(query, update);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage){ }
                return listObj;
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        /// <summary>
        /// Delete listing object based on id from the database
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <returns>return void</returns>
        public void Delete(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                Classifieds.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns top listing object collection
        /// </summary>
        /// <param name="noOfRecords">integer value for retrieving number of records for listing collection</param>
        /// <returns>Listing collection</returns>
        public List<TEntity> GetTopListings(int noOfRecords)
        {
            try
            {             
                SortByBuilder sortBuilder = new SortByBuilder();
                sortBuilder.Descending("_id");
                var result = Classifieds.FindAllAs<TEntity>().SetSortOrder(sortBuilder).SetLimit(noOfRecords);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
