using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        public IFormFile ImageFile { get; set; }
        public string Caption { get; set; }
        public int SortOrder { get; set; }
    }
}
