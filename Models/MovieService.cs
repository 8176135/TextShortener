using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace testingWebApp.Models
{
    public class MovieService
    {
        readonly IMongoCollection<Movie> _movies;

        public MovieService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MovieDb"));
            IMongoDatabase database = client.GetDatabase("testing");
            _movies = database.GetCollection<Movie>("AspNetTest");
        }

        public List<Movie> Get()
        {
            return _movies.Find(book => true).ToList();
        }

        public Movie Get(string id)
        {
            return _movies.Find<Movie>(movie => movie.ID == id).FirstOrDefault();
        }

        public Movie Create(Movie book)
        {
            _movies.InsertOne(book);
            return book;
        }

        public void Update(string id, Movie textStoreIn)
        {
            _movies.ReplaceOne(movie => movie.ID == id, textStoreIn);
        }

        public void Remove(Movie textStoreIn)
        {
            _movies.DeleteOne(movie => movie.ID == textStoreIn.ID);
        }

        public void Remove(string id)
        {
            _movies.DeleteOne(movie => movie.ID == id);
        }
    }
}