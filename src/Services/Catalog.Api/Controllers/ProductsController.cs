using Catalog.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Catalog.Api.Entites;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await this.productRepository.GetProductsAsync());
        }

        [HttpGet("{id:string}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await this.productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Route("{action}/{catalog}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductsByCatalog(string catalog)
        {
            var product = await this.productRepository.GetProductsByCatalogAsync(catalog);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await this.productRepository.CreateProductAsync(product);
            return CreatedAtRoute(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            var isUpdated = await this.productRepository.UpdateProductAsync(product);
            if (!isUpdated) return NotFound();
            return Ok(product);
        }

        [HttpDelete("{id:string}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var product = await this.productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                logger.LogWarning($"{DateTimeOffset.Now} Product with id {id} not found.");
                return NotFound();

            }

            await this.productRepository.DeleteProduct(id);
            logger.LogInformation($"{DateTimeOffset.Now} Product with id {id} is deleted.");
            logger.LogInformation(JsonSerializer.Serialize(product));
            return Ok(product);
        }

    }
}