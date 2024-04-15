using Microsoft.EntityFrameworkCore;
using EhodBoutiqueEnLigne.Data;
using EhodBoutiqueEnLigne.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EhodBoutiqueEnLigne.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EhodBDD _context;

        public OrderRepository(EhodBDD context)
        {
            _context = context;
        }

        public void Save(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChanges();
        }

        public async Task<Order> GetOrder(int? id)
        {
            var orderEntity = await _context.Order.Include(x => x.OrderLine)
                .ThenInclude(product => product.Product).SingleOrDefaultAsync(m => m.Id == id);
            return orderEntity;
        }

        public async Task<IList<Order>> GetOrders()
        {
            var orders = await _context.Order.Include(x => x.OrderLine)
                .ThenInclude(product => product.Product).ToListAsync();
            return orders;
        }
    }
}