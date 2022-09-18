using Microsoft.AspNetCore.Components;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductGetRequest
    {
        public string? Keyword { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "CategoryId")]
        public int[]? CategoryIds { get; set; }
    }
}