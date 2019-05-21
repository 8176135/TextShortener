using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
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

        public TextStore Get(byte[] id)
        {
            return _textstore.Find<TextStore>(book => book.SearchID == BsonBinaryData.Create(id)).FirstOrDefault();
        }

        public bool Create(TextStore book)
        {
            try
            {
                _textstore.InsertOne(book);
            }
            catch (MongoWriteException e)
            {
                if (e.WriteError.Code == 11000)
                {
                    return false;
                }
                throw;
            }

            return true;
        }

        public void Update(byte[] id, TextStore textStoreIn)
        {
            _textstore.ReplaceOne(book => book.SearchID == BsonBinaryData.Create(id), textStoreIn);
        }

        public void Remove(TextStore textStoreIn)
        {
            _textstore.DeleteOne(book => book.SearchID == textStoreIn.SearchID);
        }

        public void Remove(byte[] id)
        {
            _textstore.DeleteOne(book => book.SearchID == BsonBinaryData.Create(id));
        }
    }
}