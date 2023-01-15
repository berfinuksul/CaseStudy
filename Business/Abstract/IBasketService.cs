using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBasketService
    {
        IDataResult<List<Basket>> GetAll();
        IResult Create(Basket newBasket);
        IResult Update(string id, Basket updatedBasket);
        IResult Delete(string id);
    }
}
