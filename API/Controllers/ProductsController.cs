using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{    

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await productRepository.GetProductsAsync());
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.AddProduct(product);
        if (await productRepository.SavesChangesAsync()) return CreatedAtAction("GetProduct", new { id = product.Id });

        return BadRequest("Problem Creating a product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        productRepository.UpdateProduct(product);

        if (await productRepository.SavesChangesAsync()) return NoContent();

        return BadRequest("Problem Updating a product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        productRepository.DeleteProduct(product);

        if (await productRepository.SavesChangesAsync()) return NoContent();

        return BadRequest("Problem Deleting a product");
    }

    private bool ProductExists(int id)
    {
        return productRepository.ProductExists(id);
    }


}