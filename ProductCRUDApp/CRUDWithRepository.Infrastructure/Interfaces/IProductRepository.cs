using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUDWithRepository.Core;

namespace CRUDWithRepository.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int productId);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(int productId);
    }
}
