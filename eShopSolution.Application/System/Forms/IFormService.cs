using Form = eShopSolution.Data.Entities.Form;
namespace eShopSolution.Application.System.Forms
{
    public interface IFormService
    {
        IQueryable<Form> GetAll();
    }
}
