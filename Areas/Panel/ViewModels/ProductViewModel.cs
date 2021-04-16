using Microsoft.AspNetCore.Http;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.ViewModels {
    public class ProductViewModel {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string SalePrice { get; set; }
        public string OnSale { get; set; }
        public string InStock { get; set; }
        public List<string> Categories { get; set; }
        public List<Category> GetCategories { get; set; }
        public string Spec { get; set; }
        public IFormFile Img { get; set; }
        public string GetImg { get; set; }
    }
}