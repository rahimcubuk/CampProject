using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        #region Business rules
        private IResult CheckIfCategoryNameExists(string categoryName)
        {
            var result = _categoryDal.GetAll(x => x.CategoryName == categoryName).Any();
            return result ? new ErrorResult(Messages.ProductNameAlreadyExist) : (IResult)new SuccessResult();
        }
        #endregion

        public IResult Add(Category category)
        {
            IResult result = BusinessRules.Run(CheckIfCategoryNameExists(category.CategoryName));
            if (!(result is null))
            {
                return result;
            }

            _categoryDal.Add(category);
            return new SuccessResult(Messages.CategoryAdded);
        }

        public IDataResult<List<Category>> GetAll()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll(),
                                                         Messages.CategoryListed);
        }

        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>(
                _categoryDal.Get(c => c.CategoryId == categoryId)
                );
        }
    }
}
