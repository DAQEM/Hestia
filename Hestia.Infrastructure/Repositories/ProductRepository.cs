using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Infrastructure.Database;
using Hestia.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories;

public class ProductRepository(HestiaDbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetAsync(int id)
    {
        return await dbContext.Products.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await dbContext.Products.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Product> AddAsync(Product entity)
    {
        await dbContext.Products.AddAsync(entity).ConfigureAwait(false);
        
        return entity;
    }

    public async Task<Product> UpdateAsync(int id, Product entity)
    {
        Product? product = await dbContext.Products.FindAsync(id).ConfigureAwait(false);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        product.Name = entity.Name;
        product.Description = entity.Description;
        product.Summary = entity.Summary;
        product.ImageUrl = entity.ImageUrl;
        product.Downloads = entity.Downloads;
        product.GitHubUrl = entity.GitHubUrl;
        product.CurseForgeUrl = entity.CurseForgeUrl;
        product.ModrinthUrl = entity.ModrinthUrl;
        
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await dbContext.Products.FindAsync(id).ConfigureAwait(false);

        if (product is null)
        {
            return false;
        }

        dbContext.Products.Remove(product);

        return true;
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}