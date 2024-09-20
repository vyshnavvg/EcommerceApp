using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository repo;
    public ProductsController(IProductRepository repo){
        this.repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetproductById(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        
        if(product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("Createproduct", new {id = product.Id}, product);
        }

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id)) 
            return BadRequest("Cannot update this product");

        repo.UpdateProduct(product);

        if (await repo.SaveChangesAsync())
        {
          return NoContent();  
        }

        return BadRequest("Problem updating product");

    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id){

        var product = await repo.GetProductByIdAsync(id);

        if(product is null) return NotFound();

        repo.DeleteProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting product");

    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
       var brands = await repo.GetBrandsAsync();

        if( brands == null) return NotFound();

        return Ok(brands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await repo.GetTypesAsync();

        if(types == null) return NotFound();

        return Ok(types);
    }

    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}
