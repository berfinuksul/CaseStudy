using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBasketDal
    {
        List<Basket> GetAll(Expression<Func<Basket, bool>> filter = null);
        Basket Get(string id);
        void Create(Basket newBasket);
        Basket Update(string id, Basket updatedBasket);
        void Delete(string id);
    }
}
