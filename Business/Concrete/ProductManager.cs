using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        #region Construction Method 
        IProductDal _productDal;
        ICategoryService _categoryService; // Bir manager a baska bir dal enjekte edilmez. Servis enjekte edilebilir.
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        #endregion

        #region Business rules
        private int _limit = 10;
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId, int limit)
        {
            int result = GetAllByCategoryId(categoryId).Data.Count;
            return result > limit ? new ErrorResult(Messages.ProductCountOfCategoryError) : (IResult)new SuccessResult();
        }
        private IResult CheckIfProductNameExists(string name)
        {
            var result = _productDal.GetAll(x => x.ProductName == name).Any();
            return result ? new ErrorResult(Messages.ProductNameAlreadyExist) : (IResult)new SuccessResult();
        }
        private IResult CheckIfCountOfCategoryLimitExceded(int limit)
        {
            int result = _categoryService.GetAll().Data.Count;
            return result > _limit ? new ErrorResult(Messages.CategoryLimitExceded) : (IResult)new SuccessResult();
        }
        #endregion

        #region Methods

        [SecuredOperation("admin,product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(
                CheckIfProductCountOfCategoryCorrect(product.CategoryId, _limit),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCountOfCategoryLimitExceded(_limit)
                );

            if (!(result is null))
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        //[SecuredOperation("admin,user,product.list")]
        [CacheAspect(duration: 10)]
        [PerformanceAspect(5)] //--> bu metodun calismasi 5sn surerse uyari verir. 
        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect(duration: 10)]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
       
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if (product.UnitPrice < 10)
            {
                throw new Exception("");
            }
            Add(product);
            return null;
        }
        #endregion
    }
}
