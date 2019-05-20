using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace testingWebApp.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [BsonElement("ReleaseDate")]
        public DateTime ReleaseDate { get; set; }

        [BsonElement("Genre")]
        public string Genre { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }
    }
}