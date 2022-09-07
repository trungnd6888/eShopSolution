using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Categories;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface IManageCategoryService
    {
        Task<List<CategoryViewModel>> GetAll();
    }
}