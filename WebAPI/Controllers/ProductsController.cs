using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

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
        // Bu yapiyi daha once Ninject ile yapmistik. 
        // Autofac, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container kutuphaneleri
        IProductService _productManager;
        public ProductsController(IProductService productManager)
        {
            _productManager = productManager;
        }
        #endregion

        #region Controller Methods

        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            var result = _productManager.GetProductDetails();

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _productManager.GetById(id);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("get/category/{id}")]
        public IActionResult GetByCategory(int id)
        {
            var result = _productManager.GetAllByCategory(id);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post(Product product)
        {
            var result = _productManager.Add(product);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        #endregion
    }
}
