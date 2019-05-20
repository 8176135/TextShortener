using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace testingWebApp.Models
{
    public class TextStoreService
    {
        readonly IMongoCollection<TextStore> _textstore;

        public TextStoreService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MovieDb"));
            IMongoDatabase database = client.GetDatabase("testing");
            _textstore = database.GetCollection<TextStore>("AspNetTest");
        }

        public List<TextStore> Get()
        {
            return _textstore.Find(book => true).ToList();
        }

        public TextStore Get(string id)
        {
            return _textstore.Find<TextStore>(movie => movie.SearchID == id).FirstOrDefault();
        }

        public TextStore Create(TextStore book)
        {
            _textstore.InsertOne(book);
            return book;
        }

        public void Update(string id, TextStore textStoreIn)
        {
            _textstore.ReplaceOne(movie => movie.SearchID == id, textStoreIn);
        }

        public void Remove(TextStore textStoreIn)
        {
            _textstore.DeleteOne(movie => movie.SearchID == textStoreIn.SearchID);
        }

        public void Remove(string id)
        {
            _textstore.DeleteOne(movie => movie.SearchID == id);
        }
    }
}