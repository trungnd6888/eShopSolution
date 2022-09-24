using Action = eShopSolution.Data.Entities.Action;

namespace eShopSolution.Application.System.Actions
{
    public interface IActionsService
    {
        IQueryable<Action> GetAll();
    }
}
