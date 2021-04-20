using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Cart.ViewModels {
    public class CartViewModel {
        public CartProductViewModel[] items { get; set; }
        public string userId { get; set; }
    }
}
