using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Infrastructure.Data;
using ProductsSolution.Domain.Entities;

namespace ProductsSolution.API.Controllers
{
    public class CreateProductDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    [ApiController]
    [Route("api/products")]
    public class ProductsNonFastController : ControllerBase
    {
        private readonly AppDbContext DbContext;

        public ProductsNonFastController(AppDbContext db)
        {
            DbContext = db;
        }

        // Get all products (raw list)
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await DbContext.Products.ToListAsync();

            return products; // raw data
        }

        // Get product by id
        //[HttpGet("{id}")]
        [HttpGet("{id}", Name = "GetProductsByIdEndpoint")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await DbContext.Products.FindAsync(id);

            if (product == null)
                return NotFound(); //404

            return product; // raw product
        }

        // Create product
        [HttpPost]
        public async Task<ActionResult<Product>> Create(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name!,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            return CreatedAtRoute("GetProductsByIdEndpoint", new { id = product.Id }, product);
        }

        // Update product
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            var product = await DbContext.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            product.Name = updated.Name;
            product.Price = updated.Price;
            product.IsAvailable = updated.IsAvailable;

            await DbContext.SaveChangesAsync();

            return NoContent(); // no body
        }

        // Delete product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await DbContext.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            DbContext.Products.Remove(product);
            await DbContext.SaveChangesAsync();

            return NoContent(); // no body
        }
    }
}