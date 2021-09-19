using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Cart.ViewModels {
    public class CartProductViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string Img { get; set; }
        public string[] Variations { get; set; }
        
    }
}
