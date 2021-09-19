using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Store.ViewModels {
    public class CarouselProductViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string SalePrice { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public bool OnSale { get; set; }
    }
}
