using EhodBoutiqueEnLigne.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EhodBoutiqueEnLigne.Models.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        void UpdateProductStocks(int productId, int quantityToRemove);
        void SaveProduct(Product product);
        void DeleteProduct(int id);
        Task<Product> GetProduct(int id);
        Task<IList<Product>> GetProduct();
    }
}
