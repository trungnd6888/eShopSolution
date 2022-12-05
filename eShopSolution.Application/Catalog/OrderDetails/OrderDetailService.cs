using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.OrderDetails
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly EShopDbContext _context;

        public OrderDetailService(EShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<OrderDetail> GetAll()
        {
            return _context.OrderDetails;
        }

        public IQueryable<OrderDetail> GetByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(x => x.OrderId == orderId);
        }

        public void RemoveNotSave(OrderDetail orderDetail)
        {
            _context.Remove(orderDetail);
        }
    }
}
