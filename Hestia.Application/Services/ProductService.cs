using Hestia.Application.Dtos.Product;
using Hestia.Application.Result;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;

namespace Hestia.Application.Services;

public class ProductService(IProductRepository productRepository)
{
    public async Task<IResult<ProductDto?>> GetAsync(int id)
    {
        Product? product = await productRepository.GetAsync(id);
        
        if (product is null)
        {
            return new ServiceResult<ProductDto?>
            {
                Data = null,
                Success = false,
                Message = "Product not found"
            };
        }
        
        return new ServiceResult<ProductDto?>
        {
            Data = ProductDto.FromModel(product),
            Success = true,
            Message = "Product found"
        };
    }

    public async Task<IResult<IEnumerable<ProductDto>>> GetAllAsync()
    {
        IEnumerable<Product> products = await productRepository.GetAllAsync();
        
        return new ServiceResult<IEnumerable<ProductDto>>
        {
            Data = products.Select(ProductDto.FromModel),
            Success = true,
            Message = "Products found"
        };
    }

    public async Task<IResult<ProductDto>> AddAsync(ProductDto product)
    {
        try
        {
            Product addedProduct = await productRepository.AddAsync(product.ToModel());

            await productRepository.SaveChangesAsync();
            
            return new ServiceResult<ProductDto>
            {
                Data = ProductDto.FromModel(addedProduct),
                Success = true,
                Message = "Product added successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<ProductDto>
            {
                Data = null,
                Success = false,
                Message = $"An error occurred while adding the product: {ex.Message}"
            };
        }
    }

    public async Task<IResult<ProductDto>> UpdateAsync(int id, ProductDto newProduct)
    {
        try
        {
            Product updatedProduct = await productRepository.UpdateAsync(id, newProduct.ToModel());

            await productRepository.SaveChangesAsync();
            
            return new ServiceResult<ProductDto>
            {
                Data = ProductDto.FromModel(updatedProduct),
                Success = true,
                Message = "Product updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<ProductDto>
            {
                Data = null,
                Success = false,
                Message = $"An error occurred while updating the product: {ex.Message}"
            };
        }
    }

    public async Task<IResult<bool>> DeleteAsync(int id)
    {
        try
        {
            bool deleted = await productRepository.DeleteAsync(id);
            
            if (!deleted)
            {
                return new ServiceResult<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Product not deleted"
                };
            }
            
            await productRepository.SaveChangesAsync();
            
            return new ServiceResult<bool>
            {
                Data = true,
                Success = true,
                Message = "Product deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<bool>
            {
                Data = false,
                Success = false,
                Message = $"An error occurred while deleting the product: {ex.Message}"
            };
        }
    }
}