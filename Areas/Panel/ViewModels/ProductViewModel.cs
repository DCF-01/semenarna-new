using Microsoft.AspNetCore.Http;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.ViewModels {
    public class ProductViewModel {
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Price { get; set; }
        public string SalePrice { get; set; }
        public string OnSale { get; set; }
        public string InStock { get; set; }
        public string[] Categories { get; set; }
        public List<string> GetCategories { get; set; }
        public List<string> GetSpecs { get; set; }
        public string CurrentSpec { get; set; }
        public IFormFile Img { get; set; }
        public string GetImg { get; set; }
        public IFormFileCollection GalleryImages { get; set; }
        public string[] Variations { get; set; }
        public List<string> GetVariations { get; set; }
        public List<string> CurrentVariations { get; set; }

        
    }
}