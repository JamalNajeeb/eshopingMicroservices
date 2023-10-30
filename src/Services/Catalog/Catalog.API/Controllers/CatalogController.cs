using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [Route("[action]", Name = "GetProductsAsync")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Products>),(int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Products>>> GetProductsAsync()
        {
            var allProducts = await _productRepository.GetProductsAsync();
            return Ok(allProducts);
        }


        [Route("[action]/{id:length(24)}", Name = "GetProductsByProductIdAsync")]
        [HttpGet]
        //[HttpGet("{id:length(24)}", Name = "GetProductsByProductIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Products), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<Products>> GetProductById(string id)
        {
            var product = await _productRepository.GetProductsByProductIdAsync(id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductsByProductCatagoryAsync")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Products>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Products>>> GetProductByCategory(string category)
        {
            var products = await _productRepository.GetProductsByProductCatagoryAsync(category);
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductsByProductNameAsync")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Products>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Products>>> GetProductByName(string name)
        {
            var items = await _productRepository.GetProductsByProductNameAsync(name);
            if (items == null)
            {
                _logger.LogError($"Products with name: {name} not found.");
                return NotFound();
            }
            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Products), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Products>> CreateProduct([FromBody] Products product)
        {
            await _productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProductsByProductIdAsync", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Products), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Products product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Products), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }
    }
}
