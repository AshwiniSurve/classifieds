#region Imports

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endregion

namespace Classifieds.UserService.BusinessEntities
{
    public class Subscription
    {

        /// <summary>
        /// Class Name: Subcription.cs
        /// Purpose: Used as a property / DTO class to hold and pass property values.
        /// Created By: Globant
        /// Created Date: 06-Feb-2017
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string Email { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
