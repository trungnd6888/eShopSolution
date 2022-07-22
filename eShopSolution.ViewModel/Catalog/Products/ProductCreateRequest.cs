using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductCreateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Detail { get; set; }
        public double Price { get; set; }
        public int ApprovedId { get; set; }
        public int UserId { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
