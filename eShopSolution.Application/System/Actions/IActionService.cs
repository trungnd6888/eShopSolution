using Action = eShopSolution.Data.Entities.Action;

namespace eShopSolution.Application.System.Actions
{
    public interface IActionService
    {
        IQueryable<Action> GetAll();
    }
}
