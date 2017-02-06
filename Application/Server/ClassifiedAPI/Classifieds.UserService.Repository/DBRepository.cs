using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.UserService.Repository
{
    public class DBRepository : IDBRepository
    {
        #region Private Variables
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["UserProfilesConnectionString"].ConnectionString;
        private readonly string _database = ConfigurationManager.AppSettings["UserProfilesDBName"];

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

        /// <summary>
        /// gets a mongodatabase instance representing a database on the server
        /// </summary>
        protected MongoDatabase Database
        {
            get { return _server.GetDatabase(_database); }
        }

        #region Public Methods
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="ClassifiedsUser">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<ClassifiedsUser> GetCollection<ClassifiedsUser>(string name)
        {
            return _db.GetCollection<ClassifiedsUser>(name);
        }

        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="Subscription">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<Subscription> GetSubscription<Subscription>(string name)
        {
            return _db.GetCollection<Subscription>(name);
        }

        #endregion
    }
}
