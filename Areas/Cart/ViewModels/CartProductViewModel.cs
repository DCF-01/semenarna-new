using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Cart.ViewModels {
    public class CartProductViewModel {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Img { get; set; }
        
    }
}
