using application.Areas.Panel.ViewModels;
using application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Store.ViewModels {
    public class ProductViewModel {
        private double _salePrice { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double SalePrice {
            get {
                return this.OnSale ? _salePrice : 0d;
            }
            set { _salePrice = value; }
        }
        public bool OnSale { get; set; }
        public bool InStock { get; set; }
        public ICollection<Category> Categories { get; set; }
        public StoreSpecViewModel Spec { get; set; }
        public string Img { get; set; }
        public List<string> GalleryImages { get; set; }
        public  List<Variation> Variations { get; set; }
        public Product[] RelatedProducts { get; set; }
    }
}
