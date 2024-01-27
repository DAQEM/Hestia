using Hestia.Application.Dtos.Product;
using Hestia.Application.Result;
using Hestia.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController(IProductService productService, ILogger<ProductController> logger)
    : HestiaController(logger)
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        IResult<IEnumerable<ProductDto>> result = await productService.GetAllAsync();
        return HandleResult(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        IResult<ProductDto?> result = await productService.GetAsync(id);
        return HandleResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDto product)
    {
        IResult<ProductDto> result = await productService.AddAsync(product);
        return HandleResult(result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto product)
    {
        IResult<ProductDto> result = await productService.UpdateAsync(id, product);
        return HandleResult(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        IResult<bool> result = await productService.DeleteAsync(id);
        return result.Success ? Ok() : HandleFailedResult(result);
    }
}