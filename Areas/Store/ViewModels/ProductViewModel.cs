using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.ViewModels {
    public class ProductViewModel {
        public int ProductId { get; set; }
        public string[] SKUS { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SalePrice { get; set; }
        public bool OnSale { get; set; }
        public bool InStock { get; set; }
        public ICollection<Category> Categories { get; set; }
        public StoreSpecViewModel Spec { get; set; }
        public string Img { get; set; }
        public List<string> GalleryImages { get; set; }
        public  List<Variation> Variations { get; set; }
    }
}
