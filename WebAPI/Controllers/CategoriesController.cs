using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        #region ConstructorMethod
        ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region Controller Method
        
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            var result = _categoryService.GetAll();

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _categoryService.GetById(id);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post(Category product)
        {
            var result = _categoryService.Add(product);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        #endregion
    }
}
