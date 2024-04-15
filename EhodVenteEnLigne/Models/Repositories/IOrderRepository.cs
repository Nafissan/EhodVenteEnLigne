using System.Collections.Generic;
using System.Threading.Tasks;
using EhodBoutiqueEnLigne.Models.Entities;

namespace EhodBoutiqueEnLigne.Models.Repositories
{
    public interface IOrderRepository
    {
        void Save(Order order);
        Task<Order> GetOrder(int? id);
        Task<IList<Order>> GetOrders();
    }
}
