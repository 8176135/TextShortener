using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace testingWebApp.Models
{
    public class TextStore
    {
        [BsonId]
        public string SearchID { get; set; }

        [BsonElement("TextContent")]
        public string TextContent { get; set; }
    }
}