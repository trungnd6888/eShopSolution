using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Orders
{
    public interface IOrderService
    {
        IQueryable<Order> GetAll();
        Task<int> Create(Order order);
        Task<int> Update(Order order);
        Task<int> Remove(Order order);
        Task<Order> GetById(int orderId);
        Task<Order?> GetByCode(string orderCode);
        Task<int> SaveChange();
    }
}
