using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyAppDbContext _context;

        public ProductRepository(MyAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }
        public async Task<Product> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product;
        }
        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
            await Save();
        }
        public async Task Update(Product product)
        {
            var prod = await _context.Products.FindAsync(product.Id);
            if(prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                _context.Update(prod);
                await Save();
            }
        }
        public async Task Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if(product != null)
            {
                _context.Products.Remove(product);
                await Save();
            }
        }
        private async Task Save()
        {
            await _context.SaveChangesAsync(); 
        }
    }
}
