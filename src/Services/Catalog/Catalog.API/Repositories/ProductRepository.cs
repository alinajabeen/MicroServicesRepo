using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                .Products
                .Find(prop => true)
                .ToListAsync();
        }
        public async Task<Product> GetProduct(string Id)
        {
            return await _context
                .Products
                .Find(prop => prop.Id == Id)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string Name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, Name);
            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByCategory(string CategoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, CategoryName);
            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }
        public async Task InsertProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedRecord = await _context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updatedRecord.IsAcknowledged && updatedRecord.ModifiedCount > 0;
        }
        public async Task<bool> DeleteProduct(string Id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, Id);
            DeleteResult delete = await _context.Products.DeleteOneAsync(filter);
            return delete.IsAcknowledged && delete.DeletedCount > 0;
        }
    }
}
