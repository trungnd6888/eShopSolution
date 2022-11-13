using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.OrderDetails
{
    public interface IOrderDetailService
    {
        IQueryable<OrderDetail> GetByOrderId(int orderId);
        void RemoveNotSave(OrderDetail orderDetail);
    }
}
