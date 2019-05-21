using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace testingWebApp.Models
{
    public class TextStore
    {
        [BsonId]
        public BsonBinaryData SearchID { get; set; }

        [BsonElement("TextContent")]
        public BsonBinaryData TextContent { get; set; }
    }
}