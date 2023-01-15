using DataAccess.Abstract;
using Entities.Concrete;
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
    public class BasketDal : IBasketDal
    {
        private readonly IConnectionMultiplexer _redisForBasket;

        public BasketDal(IConnectionMultiplexer redisForBasket)
        {
            _redisForBasket = redisForBasket;
        }

        public void Create(Basket basket)
        {
            var newBasket = new Basket()
            {
                Id = basket.Id,
                ProductId = basket.ProductId,
                ProductName = basket.ProductName,
                ProductPrice = basket.ProductPrice
            };
            var dbase = _redisForBasket.GetDatabase();
            var json = JsonSerializer.Serialize(newBasket);

            dbase.HashSet("hashBasket", new HashEntry[] { new HashEntry(newBasket.Id, json) });
        }

        public Basket Get(string id)
        {
            var dbase = _redisForBasket.GetDatabase();
            var basket = dbase.HashGet("hashBasket", id);

            if (!string.IsNullOrEmpty(basket))
            {
                return JsonSerializer.Deserialize<Basket>(basket);
            }

            return null;
        }

        public List<Basket> GetAll(Expression<Func<Basket, bool>> filter = null)
        {
            var dbase = _redisForBasket.GetDatabase();
            var completeSet = dbase.HashGetAll("hashBasket");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Basket>(val.Value)).ToList();
                return obj;
            }

            return null;
        }

        public void Delete(string id)
        {
            var dbase = _redisForBasket.GetDatabase();
            dbase.HashDelete("hashBasket", id);
        }

        public Basket Update(string id, Basket updatedBasket)
        {
            var dbase = _redisForBasket.GetDatabase();
            var basket = dbase.HashGet("hashBasket", id);

            if (!string.IsNullOrEmpty(basket))
            {
                return JsonSerializer.Deserialize<Basket>(basket);
            }

            return null;
        }
    }
}
