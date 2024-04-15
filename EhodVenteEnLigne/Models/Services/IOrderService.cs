using System.Collections.Generic;
using System.Threading.Tasks;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.ViewModels;

namespace EhodBoutiqueEnLigne.Models.Services
{
    public interface IOrderService
    {
        void SaveOrder(OrderViewModel order);
        Task<Order> GetOrder(int id);
        Task<IList<Order>> GetOrders();
    }
}
