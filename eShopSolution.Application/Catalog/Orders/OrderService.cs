using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        public OrderService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> GetAll()
        {
            return _context.Orders;
        }

        public async Task<Order> GetById(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<int> Create(Order order)
        {
            await _context.Orders.AddAsync(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Order order)
        {
            _context.Orders.Remove(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Order order)
        {
            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByCode(string orderCode)
        {
            var query = _context.Orders.Where(x => x.Code.ToUpper() == orderCode.ToUpper().Trim());

            if (await query.CountAsync() == 0) return null;

            return await query.FirstAsync();
        }
    }
}
