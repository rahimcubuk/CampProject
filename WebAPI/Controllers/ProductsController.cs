using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //IProductService _productManager = new ProductManager(new EfProductDal()); //Bagimlilik zinciri olusturuyor. Kurucu metot ile yari bagimli bir sistem ile toparlandi.

        #region ConstructorMethod
        // IoC Container yapisi kullanilacak - Inversion of Control
        // Startup.cs -> services.AddSingleton<>();
        // Bagimliligi gidermek icin icinda data tutmadigin her yerde kullanilabilir.

        IProductService _productManager;

        public ProductsController(IProductService productManager)
        {
            _productManager = productManager;
        }
        #endregion

        [HttpGet]
        public List<Product> Get()
        {
            var result = _productManager.GetAll();
            return result.Data;
        }
    }
}
