using Entities.Concrete;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _db;

        public ApplicationDbContext(IMongoClient client, string dbName)
        {
            _db = client.GetDatabase(dbName);
        }

        public IMongoCollection<Product> Products => _db.GetCollection<Product>("products");
        public IMongoCollection<Basket> Baskets => _db.GetCollection<Basket>("baskets");
    }
}
