
using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.MastersData.Repository
{
    public class DBRepository : IDBRepository
    {
        #region Connection String

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["MasterDataConnectionString"].ConnectionString;
        private readonly string _database = ConfigurationManager.AppSettings["MasterDataDBName"];
        private readonly MongoClient _client = null;
        private readonly MongoServer _server = null;
        private readonly MongoDatabase _db = null;

        #endregion

        #region Constructor
        public DBRepository()
        {
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _db = _server.GetDatabase(_database);
        }
        #endregion

        protected MongoDatabase Database
        {
            get { return _server.GetDatabase(_database); }
        }

        #region GetCollection
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="Category">document type</typeparam>
        /// <param name="name"></param>
        /// <returns>return category collection name</returns>
        public MongoCollection<Category> GetCollection<Category>(string name)
        {
            return _db.GetCollection<Category>(name);
        }

        #endregion
    }
}
