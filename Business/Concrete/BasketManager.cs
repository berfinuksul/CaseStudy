using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BasketManager : IBasketService
    {
        private readonly IBasketDal _basketDal;

        public BasketManager(IBasketDal basketDal)
        {
            _basketDal = basketDal;
        }

        public IResult Create(Basket newBasket)
        {
            _basketDal.Create(newBasket);
            return new SuccessResult(Messages.SuccessAdd);
        }

        /* public IDataResult<Basket> Get(string id)
         {
             return new SuccessDataResult<Basket>(_basketDal.Get(x => x.Id == id), Messages.SuccessListed);
         }*/

        public IDataResult<List<Basket>> GetAll()
        {
            return new SuccessDataResult<List<Basket>>(_basketDal.GetAll(), Messages.SuccessListed);
        }

        public IResult Delete(string id)
        {
            _basketDal.Delete(id);
            return new SuccessResult(Messages.SuccessDelete);
        }

        public IResult Update(string id, Basket updatedBasket)
        {
            _basketDal.Update(id, updatedBasket);
            return new SuccessResult(Messages.SuccessUpdate);
        }
    }
}
