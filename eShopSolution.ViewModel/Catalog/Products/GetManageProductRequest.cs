using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class GetManageProductRequest
    {
        public string? Keyword { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "CategoryId")]
        public int[]? CategoryIds { get; set; }
    }
}