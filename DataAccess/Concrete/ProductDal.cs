using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class ProductDal : IProductDal
    {
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly IConnectionMultiplexer _redisForBasket;

        public ProductDal(IOptions<MongoDbSettings> mongoDbSettings, IConnectionMultiplexer redisForBasket)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _productsCollection = mongoDatabase.GetCollection<Product>(mongoDbSettings.Value.ProductsCollectionName);

            _redisForBasket = redisForBasket;
        }

        public void AddBasket(string productId, Basket basket)
        {
            var addBasket = _productsCollection.Find(x => x.Id == productId).SingleOrDefault();

            var newBasket = new Basket()
            {
                Id = basket.Id,
                ProductId = addBasket.Id,
                ProductName = addBasket.Name,
                ProductPrice = addBasket.Price
            };
            var dbase = _redisForBasket.GetDatabase();
            var json = JsonSerializer.Serialize(newBasket);

            dbase.HashSet("hashForBasket", new HashEntry[] { new HashEntry(newBasket.Id, json) });
        }

        public void Create(Product newProduct)
        {
            _productsCollection.InsertOne(newProduct);
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            return _productsCollection.Find(filter).FirstOrDefault();
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            return _productsCollection.Find(_ => true).ToList();
        }

        public void Delete(string id)
        {
            _productsCollection.DeleteOne(x => x.Id == id);
        }

        public void Update(string id, Product updatedProduct)
        {
            _productsCollection.ReplaceOne(x => x.Id == id, updatedProduct);
        }
    }
}
