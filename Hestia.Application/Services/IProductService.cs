using Hestia.Application.Dtos.Product;
using Hestia.Application.Result;

namespace Hestia.Application.Services;

public interface IProductService
{
    Task<IResult<ProductDto?>> GetAsync(int id);
    Task<IResult<IEnumerable<ProductDto>>> GetAllAsync();
    Task<IResult<ProductDto>> AddAsync(ProductDto product);
    Task<IResult<ProductDto>> UpdateAsync(int id, ProductDto product);
    Task<IResult<bool>> DeleteAsync(int id);
}