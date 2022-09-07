using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Distributors;

namespace eShopSolution.Application.Catalog.Distributors
{
    public interface IManageDistributorService
    {
        Task<List<DistributorViewModel>> GetAll();
    }
}