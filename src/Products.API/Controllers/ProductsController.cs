using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Core.Entities;
using Products.Core.Interfaces;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            this._productsService = productsService;
        }

        // GET: api/Products
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var products = this._productsService.GetProducts();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }

        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var product = this._productsService.GetProduct(id);
                return Ok(product);
            } catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        // POST: api/Products
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                this._productsService.AddProduct(product);
                return Ok(product);
            } catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            try
            {
                product.Id = id;
                this._productsService.UpdateProduct(product);
                return Ok(product);
            } catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                this._productsService.DeleteProduct(id);
                return Ok();
            } catch (Exception e)
            {
                return BadRequest("Error : " + e.Message);
            }
        }
    }
}
